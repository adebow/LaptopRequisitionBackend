using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Notification> GetByIdAsync(Guid id)
        {
            return await _context.Notifications
                                 .Include(n => n.Employee)
                                 .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.Notifications
                                 .Include(n => n.Employee)
                                 .Where(n => n.EmployeeId == employeeId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications
                                 .Include(n => n.Employee)
                                 .ToListAsync();
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

        public async Task DeleteAsync(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}