using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using Microsoft.EntityFrameworkCore;


namespace LaptopRequisition.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordResetTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken> GetByIdAsync(Guid id)
        {
            return await _context.PasswordResetTokens
                                 .Include(prt => prt.Employee)
                                 .FirstOrDefaultAsync(prt => prt.Id == id);
        }

        public async Task<PasswordResetToken> GetByTokenAsync(string token)
        {
            return await _context.PasswordResetTokens
                                 .Include(prt => prt.Employee)
                                 .FirstOrDefaultAsync(prt => prt.Token == token);
        }

        public async Task<IEnumerable<PasswordResetToken>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.PasswordResetTokens
                                 .Include(prt => prt.Employee)
                                 .Where(prt => prt.EmployeeId == employeeId)
                                 .ToListAsync();
        }

        public async Task AddAsync(PasswordResetToken passwordResetToken)
        {
            await _context.PasswordResetTokens.AddAsync(passwordResetToken);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PasswordResetToken passwordResetToken)
        {
            _context.PasswordResetTokens.Update(passwordResetToken);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var passwordResetToken = await _context.PasswordResetTokens.FindAsync(id);
            if (passwordResetToken != null)
            {
                _context.PasswordResetTokens.Remove(passwordResetToken);
                await _context.SaveChangesAsync();
            }
        }
    }
}