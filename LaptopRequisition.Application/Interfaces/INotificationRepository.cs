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
        Task<IEnumerable<Notification>> GetLatestByEmployeeIdAsync(Guid employeeId, int count); // Added this line
    }
}