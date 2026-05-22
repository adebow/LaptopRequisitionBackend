using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Domain;


namespace LaptopRequisition.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Employee> RegisterEmployeeAsync(RegisterEmployeeDto registerDto);
        Task<string> LoginAsync(string email, string password);
        Task<bool> RequestPasswordResetAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> ChangePasswordAsync(Guid employeeId, string currentPassword, string newPassword);
    }
}