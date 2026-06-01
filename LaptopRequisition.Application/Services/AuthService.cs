using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.DTOs.Notification;
using LaptopRequisition.Application.DTOs.SSO;
using LaptopRequisition.Application.Helpers;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Application.Interfaces.External;
using LaptopRequisition.Application.Interfaces.SSO;
using LaptopRequisition.Domain;
using LaptopRequisition.Domain.Enums;
using LaptopRequisition.Application.Configurations; // Added for all settings
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Refit;
using LaptopRequisition.Application.DTOs.Login; // Added for LoginResponseDto

namespace LaptopRequisition.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISsoClient _ssoClient;
        private readonly SsoSettings _ssoSettings;
        private readonly IOtpHelperService _otpHelperService;
        private readonly INotificationApi _notificationApi;
        private readonly NotificationApiSettings _notificationApiSettings;
        private readonly IRoleRepository _roleRepository;
        private readonly AuthSettings _authSettings; // Added

        public AuthService(IEmployeeRepository employeeRepository,
                           IJwtService jwtService,
                           IPasswordResetTokenRepository passwordResetTokenRepository,
                           IDepartmentRepository departmentRepository,
                           ISsoClient ssoClient,
                           IOptions<SsoSettings> ssoSettingsOptions,
                           IOtpHelperService otpHelperService,
                           INotificationApi notificationApi,
                           IOptions<NotificationApiSettings> notificationApiSettingsOptions,
                           IRoleRepository roleRepository,
                           IOptions<AuthSettings> authSettingsOptions) // Added
        {
            _employeeRepository = employeeRepository;
            _jwtService = jwtService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _departmentRepository = departmentRepository;
            _ssoClient = ssoClient;
            _ssoSettings = ssoSettingsOptions.Value;
            _otpHelperService = otpHelperService;
            _notificationApi = notificationApi;
            _notificationApiSettings = notificationApiSettingsOptions.Value;
            _roleRepository = roleRepository;
            _authSettings = authSettingsOptions.Value; // Initialized
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

            // Fetch the default "Employee" role ID
            var employeeRole = await _roleRepository.GetByNameAsync("Employee");
            if (employeeRole == null)
            {
                throw new InvalidOperationException("Default 'Employee' role not found. Please ensure roles are seeded.");
            }
         
            var newEmployeeId = Guid.NewGuid();
            
            var ssoUserRequest = new SsoUserCreationRequestDto
            {
                Username = newEmployeeId.ToString(), 
                Password = registerDto.Password,
                Email = registerDto.Email,
                SourceId = newEmployeeId.ToString() 
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
                RoleId = employeeRole.Id, // Assign the default Employee RoleId
                PasswordHash = string.Empty, // Password is managed by SSO
                FailedLoginCount = 0,
                IsLocked = false,
                PreviousPasswordHashes = JsonSerializer.Serialize(new List<string>()),
                IsVerified = false // Account is unverified until OTP is confirmed
            };

            await _employeeRepository.AddAsync(employee);
            
            return employee;
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            var response = new LoginResponseDto { IsSuccess = false };
            var employee = await _employeeRepository.GetByEmailWithDepartmentAndRoleAsync(email);

            if (employee == null)
            {
                response.Message = "Invalid credentials.";
                return response;
            }

            response.EmployeeDetails = MapEmployeeToDto(employee);
            response.IsFirstLogin = employee.IsFirstLogin;
            response.IsAdmin = (employee.Role?.Name == "Admin"); // Determine if admin

            if (employee.IsLocked)
            {
                if (employee.LockoutEndDate.HasValue && employee.LockoutEndDate > DateTime.UtcNow)
                {
                    response.Message = $"Account is locked. Try again after {employee.LockoutEndDate.Value.ToLocalTime()}.";
                    response.IsLocked = true;
                    response.LockoutEndDate = employee.LockoutEndDate;
                    return response;
                }
                else
                {
                    // Lockout period expired, reset lockout
                    employee.IsLocked = false;
                    employee.FailedLoginCount = 0;
                    employee.LockoutEndDate = null;
                    await _employeeRepository.UpdateLoginAttemptsAsync(employee); // Use new method
                }
            }

            if (!employee.IsVerified)
            {
                response.Message = "Account not verified. Please verify your account first.";
                return response;
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
                response.TokenDetails = ssoTokenResponse;
                response.IsSuccess = true;
                response.Message = "Login successful.";

                // Reset failed login attempts on successful login
                if (employee.FailedLoginCount > 0 || employee.IsLocked)
                {
                    employee.FailedLoginCount = 0;
                    employee.IsLocked = false;
                    employee.LockoutEndDate = null;
                    await _employeeRepository.UpdateLoginAttemptsAsync(employee); // Use new method
                }
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    employee.FailedLoginCount++;
                    // Update response with current count (though not directly used in DTO, good for debugging)
                    // response.FailedLoginCount = employee.FailedLoginCount; 

                    if (employee.FailedLoginCount >= _authSettings.MaxFailedLoginAttempts)
                    {
                        employee.IsLocked = true;
                        employee.LockoutEndDate = DateTime.UtcNow.AddMinutes(_authSettings.LockoutDurationMinutes);
                        response.IsLocked = true;
                        response.LockoutEndDate = employee.LockoutEndDate;
                        response.Message = $"Account locked due to too many failed attempts. Try again after {employee.LockoutEndDate.Value.ToLocalTime()}.";
                    }
                    else
                    {
                        response.Message = "Invalid credentials.";
                    }
                    await _employeeRepository.UpdateLoginAttemptsAsync(employee); // Use new method
                }
                else
                {
                    response.Message = $"Failed to authenticate with SSO system. Status: {ex.StatusCode}. Message: {ex.Content}";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"An unexpected error occurred during SSO authentication: {ex.Message}";
            }
            
            return response;
        }

        public async Task<LoginResponseDto> AdminLoginAsync(string email, string password)
        {
            var loginResponse = await LoginAsync(email, password);

            if (!loginResponse.IsSuccess)
            {
                return loginResponse; // Return early if regular login failed or account is locked/unverified
            }

            // If login was successful, check for Admin role
            if (!loginResponse.IsAdmin)
            {
                loginResponse.IsSuccess = false;
                loginResponse.Message = "Access denied. Only administrators can log in to the Admin Portal.";
                // Optionally, clear token details if not an admin
                loginResponse.TokenDetails = new SsoTokenResponseDto();
                loginResponse.EmployeeDetails = new EmployeeDto();
            }

            return loginResponse;
        }

        public async Task VerifyAccountAsync(string validationReference, string otp)
        {
            var otpValidationResult = await _otpHelperService.ValidateOtpAsync(validationReference, otp);

            if (!otpValidationResult.IsSuccessful || otpValidationResult.Data?.Data?.UserReference == null)
            {
                throw new InvalidOperationException(otpValidationResult.Message ?? "OTP verification failed.");
            }

            var employeeEmail = otpValidationResult.Data.Data.UserReference;
            var employee = await _employeeRepository.GetByEmailWithDepartmentAndRoleAsync(employeeEmail);

            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found for the provided OTP reference.");
            }

            if (employee.IsVerified)
            {
                throw new InvalidOperationException("Account is already verified.");
            }

            employee.IsVerified = true;
            employee.UpdatedAt = DateTime.UtcNow;
            await _employeeRepository.UpdateAsync(employee);

            // Send welcome email after successful verification
            var welcomeEmailBody = await BuildWelcomeEmailBodyAsync(employee.FullName);
            var notificationRequest = new NotificationRequest
            {
                Channels = new List<string> { "Email" },
                From = _notificationApiSettings.FromEmail,
                To = employee.Email,
                Subject = "Welcome to Laptop Requisition System",
                Message = welcomeEmailBody
            };
            var notificationResponse = await _notificationApi.SendNotificationAsync(notificationRequest);

            if (!notificationResponse.IsSuccessStatusCode || notificationResponse.Content is null || !notificationResponse.Content.IsSuccessful)
            {
                // Log this error, but don't prevent account verification if email fails
                // Consider a retry mechanism or admin notification for failed welcome emails
                Console.WriteLine($"Warning: Failed to send welcome email to {employee.Email}: {notificationResponse.Error?.Content}");
            }
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailWithDepartmentAndRoleAsync(email);
            if (employee == null)
            {
                return true; // Security by obscurity
            }

            if (employee.PasswordHash == string.Empty) // SSO managed user
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
            // Replaced direct email service with Notification API
            var emailBody = await BuildPasswordResetEmailBodyAsync(resetLink, employee.FullName);
            var notificationRequest = new NotificationRequest
            {
                Channels = new List<string> { "Email" },
                From = _notificationApiSettings.FromEmail,
                To = employee.Email,
                Subject = "Password Reset Request",
                Message = emailBody
            };
            var notificationResponse = await _notificationApi.SendNotificationAsync(notificationRequest);

            if (!notificationResponse.IsSuccessStatusCode || notificationResponse.Content is null || !notificationResponse.Content.IsSuccessful)
            {
                // Log error if notification failed
                throw new InvalidOperationException($"Failed to send password reset email: {notificationResponse.Error?.Content}");
            }

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var resetToken = await _passwordResetTokenRepository.GetByTokenAsync(token);

            if (resetToken == null || resetToken.IsUsed || resetToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Invalid or expired password reset token.");
            }

            // Safely access EmployeeId from the nullable property
            if (!resetToken.EmployeeId.HasValue)
            {
                throw new InvalidOperationException("Password reset token is not associated with an employee.");
            }

            var employee = await _employeeRepository.GetByIdWithDepartmentAndRoleAsync(resetToken.EmployeeId.Value);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found for the given token.");
            }

            if (employee.PasswordHash == string.Empty) // SSO managed user
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
            var employee = await _employeeRepository.GetByIdWithDepartmentAndRoleAsync(employeeId);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }
            
            if (employee.PasswordHash == string.Empty) // SSO managed user
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
            // Department and Role are now eagerly loaded, so no need for .Result calls
            return new EmployeeDto
            {
                Id = employee.Id,
                StaffId = employee.StaffId,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? "Unknown", // Access directly from loaded Department
                Role = employee.Role?.Name ?? "Unknown", // Access directly from loaded Role
                IsLocked = employee.IsLocked
            };
        }

        // Helper method to build password reset email body from template
        private async Task<string> BuildPasswordResetEmailBodyAsync(string resetLink, string employeeName)
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "PasswordReset.html");
            if (!File.Exists(templatePath))
            {
                // Fallback to a simple message if template not found
                return $"Dear {employeeName},\n\nYour password reset link is: {resetLink}\n\nBest regards,\nLRS Team";
            }
            var body = await File.ReadAllTextAsync(templatePath);
            return body
                .Replace("{{employeeName}}", employeeName)
                .Replace("{{resetLink}}", resetLink);
        }

        // Helper method to build welcome email body from template
        private async Task<string> BuildWelcomeEmailBodyAsync(string employeeName)
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "Welcome.html");
            if (!File.Exists(templatePath))
            {
                // Fallback to a simple message if template not found
                return $"Dear {employeeName},\n\nYour account has been successfully created. You can now log in to request laptops.\n\nBest regards,\nLRS Team";
            }
            var body = await File.ReadAllTextAsync(templatePath);
            return body
                .Replace("{{employeeName}}", employeeName);
        }
    }
}