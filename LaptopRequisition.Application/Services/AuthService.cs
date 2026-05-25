using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.DTOs.SSO;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Application.Interfaces.SSO;
using LaptopRequisition.Domain;
using LaptopRequisition.Application.Configurations;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Refit;
using BCrypt.Net; 

namespace LaptopRequisition.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IEmailService _emailService;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISsoClient _ssoClient;
        private readonly SsoSettings _ssoSettings;

        public AuthService(IEmployeeRepository employeeRepository,
                           IJwtService jwtService,
                           IPasswordResetTokenRepository passwordResetTokenRepository,
                           IEmailService emailService,
                           IDepartmentRepository departmentRepository,
                           ISsoClient ssoClient,
                           IOptions<SsoSettings> ssoSettingsOptions)
        {
            _employeeRepository = employeeRepository;
            _jwtService = jwtService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _emailService = emailService;
            _departmentRepository = departmentRepository;
            _ssoClient = ssoClient;
            _ssoSettings = ssoSettingsOptions.Value;
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

            // Generate a new GUID for the employee (which will be used as SSO username/sourceId)
            var newEmployeeId = Guid.NewGuid();

            // 1. Register user in SSO system
            var ssoUserRequest = new SsoUserCreationRequestDto
            {
                Username = newEmployeeId.ToString(), // Use employee ID as SSO username
                Password = registerDto.Password,
                Email = registerDto.Email,
                SourceId = newEmployeeId.ToString() // Use employee ID as SourceId
            };

            try
            {
                var ssoResponse = await _ssoClient.CreateSsoUser(_ssoSettings.ClientId, ssoUserRequest);
                if (!ssoResponse.IsSuccess)
                {
                    throw new InvalidOperationException($"SSO user creation failed: {ssoResponse.Message ?? "Unknown error"}");
                }
            }
            catch (ApiException ex)
            {
                
                throw new InvalidOperationException($"Failed to create user in SSO system. Status: {ex.StatusCode}. Message: {ex.Content}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred during SSO user creation: {ex.Message}", ex);
            }
            
            var employee = new Employee
            {
                Id = newEmployeeId, 
                StaffId = registerDto.StaffId,
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DepartmentId = registerDto.DepartmentId, 
                Role = registerDto.Role, 
                PasswordHash = string.Empty,
                FailedLoginCount = 0,
                IsLocked = false,
                PreviousPasswordHashes = JsonSerializer.Serialize(new List<string>()) // No local password history
            };

            await _employeeRepository.AddAsync(employee);
            
            await _emailService.SendEmailAsync(employee.Email, "Welcome to Laptop Requisition System", $"Dear {employee.FullName},\n\nYour account has been successfully created. You can now log in to request laptops.\n\nBest regards,\nLRS Team");

            return employee;
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password) 
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
            
            var ssoTokenRequest = new SsoTokenRequestDto
            {
                ClientId = _ssoSettings.ClientId,
                ClientSecret = _ssoSettings.ClientSecret,
                Username = employee.Id.ToString(),
                Password = password
            };

            SsoTokenResponseDto ssoTokenResponse;
            try
            {
                ssoTokenResponse = await _ssoClient.GetSsoToken(ssoTokenRequest);
            }
            catch (ApiException ex)
            {
                // Handle Refit API errors (e.g., invalid credentials, network issues)
                // For invalid credentials, SSO might return 400 Bad Request
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Increment failed login count for local employee if SSO rejects
                    employee.FailedLoginCount++;
                    if (employee.FailedLoginCount >= 5)
                    {
                        employee.IsLocked = true;
                    }
                    await _employeeRepository.UpdateAsync(employee);
                    throw new UnauthorizedAccessException("Invalid credentials.");
                }
                throw new UnauthorizedAccessException($"Failed to authenticate with SSO system. Status: {ex.StatusCode}. Message: {ex.Content}", ex);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException($"An unexpected error occurred during SSO authentication: {ex.Message}", ex);
            }
            
            employee.FailedLoginCount = 0;
            await _employeeRepository.UpdateAsync(employee); 

            
            var localJwtToken = _jwtService.GenerateToken(employee);
            
            var employeeDto = MapEmployeeToDto(employee);

            return new LoginResponseDto
            {
                TokenDetails = ssoTokenResponse,
                EmployeeDetails = employeeDto 
            };
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);
            if (employee == null)
            {
                return true; 
            }

            if (employee.PasswordHash == string.Empty) 
            {
                throw new InvalidOperationException("Password reset for this account is managed by the SSO system. Please use the SSO portal's password reset functionality.");
            }

            var token = Guid.NewGuid().ToString();
            var passwordResetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
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

            if (employee.PasswordHash == string.Empty)
            {
                throw new InvalidOperationException("Password reset for this account is managed by the SSO system. Please use the SSO portal's password reset functionality.");
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
            
            if (employee.PasswordHash == string.Empty)
            {
                throw new InvalidOperationException("Password change for this account is managed by the SSO system. Please use the SSO portal's password change functionality.");
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

        private EmployeeDto MapEmployeeToDto(Employee employee)
        {
            var department = _departmentRepository.GetByIdAsync(employee.DepartmentId).Result;
            
            return new EmployeeDto
            {
                Id = employee.Id,
                StaffId = employee.StaffId,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DepartmentId = employee.DepartmentId,
                DepartmentName = department?.Name ?? "Unknown",
                Role = employee.Role,
                IsLocked = employee.IsLocked
            };
        }
    }
}