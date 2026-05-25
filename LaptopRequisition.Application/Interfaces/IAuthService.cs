using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Domain;
using System.Threading.Tasks; // Added for Task

namespace LaptopRequisition.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Employee> RegisterEmployeeAsync(RegisterEmployeeDto registerDto);
        Task<LoginResponseDto> LoginAsync(string email, string password); // Changed return type
        Task<bool> RequestPasswordResetAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> ChangePasswordAsync(Guid employeeId, string currentPassword, string newPassword);
    }
}