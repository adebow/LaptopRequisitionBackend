using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // Removed: public async Task<IEnumerable<Notification>> GetLatestByEmployeeIdAsync(Guid employeeId, int count)
        // {
        //     return await _context.Notifications
        //                         .Where(n => n.EmployeeId == employeeId)
        //                         .OrderByDescending(n => n.CreatedAt)
        //                         .Take(count)
        //                         .ToListAsync();
        // }

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

        // New methods for DashboardService
        public async Task<int> CountUnreadByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.Notifications
                .CountAsync(n => n.EmployeeId == employeeId && !n.IsRead);
        }

        public async Task<IEnumerable<Notification>> GetRecentNotificationsByEmployeeIdAsync(Guid employeeId, int count)
        {
            return await _context.Notifications
                .Where(n => n.EmployeeId == employeeId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}