using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using System;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Text.Json; // Added for JSON serialization/deserialization
using System.Collections.Generic;
using System.Linq;

namespace LaptopRequisition.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IEmailService _emailService;
        private readonly IDepartmentRepository _departmentRepository; // Added

        public AuthService(IEmployeeRepository employeeRepository,
                           IJwtService jwtService,
                           IPasswordResetTokenRepository passwordResetTokenRepository,
                           IEmailService emailService,
                           IDepartmentRepository departmentRepository) // Added
        {
            _employeeRepository = employeeRepository;
            _jwtService = jwtService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _emailService = emailService;
            _departmentRepository = departmentRepository; // Assigned
        }

        public async Task<Employee> RegisterEmployeeAsync(RegisterEmployeeDto registerDto)
        {
            // Validate unique StaffId and Email
            var existingEmployeeByStaffId = await _employeeRepository.GetByStaffIdAsync(registerDto.StaffId);
            if (existingEmployeeByStaffId != null)
            {
                throw new InvalidOperationException("Staff ID is already registered.");
            }

            var existingEmployeeByEmail = await _employeeRepository.GetByEmailAsync(registerDto.Email);
            if (existingEmployeeByEmail != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            // Validate DepartmentId exists
            var department = await _departmentRepository.GetByIdAsync(registerDto.DepartmentId); // Changed to GetByIdAsync
            if (department == null)
            {
                throw new InvalidOperationException($"Department with ID '{registerDto.DepartmentId}' not found."); // Updated error message
            }

            // Hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                StaffId = registerDto.StaffId,
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DepartmentId = registerDto.DepartmentId, // Directly assign DepartmentId
                Role = registerDto.Role, // Role remains string for now
                PasswordHash = passwordHash,
                FailedLoginCount = 0,
                IsLocked = false,
                PreviousPasswordHashes = JsonSerializer.Serialize(new List<string> { passwordHash }) // Store initial password hash
            };

            await _employeeRepository.AddAsync(employee);
            return employee;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);

            if (employee == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            if (employee.IsLocked)
            {
                throw new UnauthorizedAccessException("Account is locked. Please contact support.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash))
            {
                employee.FailedLoginCount++;
                if (employee.FailedLoginCount >= 5)
                {
                    employee.IsLocked = true;
                }
                await _employeeRepository.UpdateAsync(employee);
                throw new UnauthorizedAccessException("Invalid credentials.");
            }
            
            employee.FailedLoginCount = 0;
            await _employeeRepository.UpdateAsync(employee);
            
            return _jwtService.GenerateToken(employee);
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);
            if (employee == null)
            {
                // For security, don't reveal if the email exists or not
                return true;
            }

            var token = Guid.NewGuid().ToString();
            var passwordResetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
                IsUsed = false
            };

            await _passwordResetTokenRepository.AddAsync(passwordResetToken);

            // In a real application, construct a proper reset link
            var resetLink = $"https://yourdomain.com/reset-password?token={token}";
            await _emailService.SendEmailAsync(employee.Email, "Password Reset Request", $"Please use the following link to reset your password: {resetLink}");

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var resetToken = await _passwordResetTokenRepository.GetByTokenAsync(token);

            if (resetToken == null || resetToken.IsUsed || resetToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Invalid or expired password reset token.");
            }

            var employee = await _employeeRepository.GetByIdAsync(resetToken.EmployeeId);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found for the given token.");
            }
            
            var previousHashes = JsonSerializer.Deserialize<List<string>>(employee.PreviousPasswordHashes)
                                 ?? new List<string>(); // Modified line
            foreach (var oldHash in previousHashes)
            {
                if (BCrypt.Net.BCrypt.Verify(newPassword, oldHash))
                {
                    throw new InvalidOperationException("New password cannot be one of the last 3 used passwords.");
                }
            }

           
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            
            previousHashes.Add(newPasswordHash);
            if (previousHashes.Count > 3)
            {
                previousHashes.RemoveAt(0); // Keep only the last 3
            }
            employee.PreviousPasswordHashes = JsonSerializer.Serialize(previousHashes);

            // Update employee password and reset login related fields
            employee.PasswordHash = newPasswordHash;
            employee.FailedLoginCount = 0;
            employee.IsLocked = false;

            // Mark token as used
            resetToken.IsUsed = true;

            await _employeeRepository.UpdateAsync(employee);
            await _passwordResetTokenRepository.UpdateAsync(resetToken);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid employeeId, string currentPassword, string newPassword)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, employee.PasswordHash))
            {
                throw new UnauthorizedAccessException("Incorrect current password.");
            }

            // Check against last 3 passwords
            var previousHashes = JsonSerializer.Deserialize<List<string>>(employee.PreviousPasswordHashes)
                                 ?? new List<string>(); // Modified line
            foreach (var oldHash in previousHashes)
            {
                if (BCrypt.Net.BCrypt.Verify(newPassword, oldHash))
                {
                    throw new InvalidOperationException("New password cannot be one of the last 3 used passwords.");
                }
            }

            // Hash new password
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Update previous password hashes
            previousHashes.Add(newPasswordHash);
            if (previousHashes.Count > 3)
            {
                previousHashes.RemoveAt(0); // Keep only the last 3
            }
            employee.PreviousPasswordHashes = JsonSerializer.Serialize(previousHashes);

            // Update employee password and reset login related fields
            employee.PasswordHash = newPasswordHash;
            employee.FailedLoginCount = 0;
            employee.IsLocked = false;

            await _employeeRepository.UpdateAsync(employee);

            return true;
        }
    }
}