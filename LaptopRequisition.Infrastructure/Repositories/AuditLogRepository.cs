using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using System.Threading.Tasks;

namespace LaptopRequisition.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}