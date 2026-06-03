using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(Guid id);
        Task<IEnumerable<Notification>> GetByEmployeeIdAsync(Guid employeeId, bool unreadOnly = false);
        Task<IEnumerable<Notification>> GetAllAsync();
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Guid id);
        Task UpdateRangeAsync(IEnumerable<Notification> notifications);
        // Removed: Task<IEnumerable<Notification>> GetLatestByEmployeeIdAsync(Guid employeeId, int count);
        Task<int> CountUnreadByEmployeeIdAsync(Guid employeeId); // Added
        Task<IEnumerable<Notification>> GetRecentNotificationsByEmployeeIdAsync(Guid employeeId, int count); // Added
    }
}