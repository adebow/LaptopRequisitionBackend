using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using Microsoft.EntityFrameworkCore;


namespace LaptopRequisition.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<Notification?> GetByIdAsync(Guid id) // Changed return type to Notification?
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetByEmployeeIdAsync(
Guid employeeId, bool unreadOnly = false)
        {
            var query = _context.Notifications
                                .Where(n => n.EmployeeId == employeeId);

            if (unreadOnly)
            {
                query = query.Where(n => !n.IsRead);
            }

            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetLatestByEmployeeIdAsync(Guid employeeId, int count)
        {
            return await _context.Notifications
                                .Where(n => n.EmployeeId == employeeId)
                                .OrderByDescending(n => n.CreatedAt)
                                .Take(count)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllAsync() // Implemented GetAllAsync
        {
            return await _context.Notifications.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task DeleteAsync(Guid id) // Implemented DeleteAsync
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<Notification> notifications)
        {
            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
        }
    }
}