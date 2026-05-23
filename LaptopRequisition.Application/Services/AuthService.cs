using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using System.Text.Json; 

namespace LaptopRequisition.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IEmailService _emailService;
        private readonly IDepartmentRepository _departmentRepository;

        public AuthService(IEmployeeRepository employeeRepository,
                           IJwtService jwtService,
                           IPasswordResetTokenRepository passwordResetTokenRepository,
                           IEmailService emailService,
                           IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _jwtService = jwtService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _emailService = emailService;
            _departmentRepository = departmentRepository;
        }

        public async Task<Employee> RegisterEmployeeAsync(RegisterEmployeeDto registerDto)
        {
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
            
            var department = await _departmentRepository.GetByIdAsync(registerDto.DepartmentId); 
            if (department == null)
            {
                throw new InvalidOperationException($"Department with ID '{registerDto.DepartmentId}' not found."); 
            }
            
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                StaffId = registerDto.StaffId,
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DepartmentId = registerDto.DepartmentId, 
                Role = registerDto.Role, 
                PasswordHash = passwordHash,
                FailedLoginCount = 0,
                IsLocked = false,
                PreviousPasswordHashes = JsonSerializer.Serialize(new List<string> { passwordHash }) 
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
                return true;
            }

            var token = Guid.NewGuid().ToString();
            var passwordResetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1), 
                IsUsed = false
            };

            await _passwordResetTokenRepository.AddAsync(passwordResetToken);
            
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
                                 ?? new List<string>(); 
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
                previousHashes.RemoveAt(0); 
            }
            employee.PreviousPasswordHashes = JsonSerializer.Serialize(previousHashes);
            
            employee.PasswordHash = newPasswordHash;
            employee.FailedLoginCount = 0;
            employee.IsLocked = false;
            
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
            
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, employee.PasswordHash))
            {
                throw new UnauthorizedAccessException("Incorrect current password.");
            }
            
            var previousHashes = JsonSerializer.Deserialize<List<string>>(employee.PreviousPasswordHashes)
                                 ?? new List<string>(); 
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
                previousHashes.RemoveAt(0); 
            }
            employee.PreviousPasswordHashes = JsonSerializer.Serialize(previousHashes);
            
            employee.PasswordHash = newPasswordHash;
            employee.FailedLoginCount = 0;
            employee.IsLocked = false;

            await _employeeRepository.UpdateAsync(employee);

            return true;
        }
    }
}