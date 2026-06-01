using LaptopRequisition.Domain;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog auditLog);
    }
}