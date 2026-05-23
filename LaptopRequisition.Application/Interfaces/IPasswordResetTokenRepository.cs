using LaptopRequisition.Domain;


namespace LaptopRequisition.Application.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task<PasswordResetToken> GetByIdAsync(Guid id);
        Task<PasswordResetToken> GetByTokenAsync(string token);
        Task<IEnumerable<PasswordResetToken>> GetByEmployeeIdAsync(Guid employeeId);
        Task AddAsync(PasswordResetToken passwordResetToken);
        Task UpdateAsync(PasswordResetToken passwordResetToken);
        Task DeleteAsync(Guid id);
    }
}