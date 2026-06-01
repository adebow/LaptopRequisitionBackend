using LaptopRequisition.Application.DTOs.Notification;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public NotificationService(INotificationRepository notificationRepository, IEmployeeRepository employeeRepository)
        {
            _notificationRepository = notificationRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task CreateNotificationAsync(Guid employeeId, string message)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId, 
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification);
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByEmployeeIdAsync(Guid employeeId, bool unreadOnly = false)
        {
            var notifications = await _notificationRepository.GetByEmployeeIdAsync(employeeId, unreadOnly);
            var employee = await _employeeRepository.GetByIdAsync(employeeId); 

            return notifications.Select(n => MapToDto(n, employee?.FullName));
        }

        // Renamed and updated to use GetRecentNotificationsByEmployeeIdAsync
        public async Task<IEnumerable<NotificationResponseDto>> GetRecentNotificationsByEmployeeIdAsync(Guid employeeId, int count)
        {
            var notifications = await _notificationRepository.GetRecentNotificationsByEmployeeIdAsync(employeeId, count);
            var employee = await _employeeRepository.GetByIdAsync(employeeId); 

            return notifications.Select(n => MapToDto(n, employee?.FullName));
        }
        
        public async Task<NotificationResponseDto> GetNotificationByIdAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                return null; 
            }
            // Handle nullable EmployeeId
            var employee = notification.EmployeeId.HasValue ? await _employeeRepository.GetByIdAsync(notification.EmployeeId.Value) : null;
            return MapToDto(notification, employee?.FullName);
        }

        public async Task MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new InvalidOperationException("Notification not found.");
            }
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
        }

        
        public async Task MarkAllNotificationsAsReadAsync(Guid employeeId)
        {
            var notifications = await _notificationRepository.GetByEmployeeIdAsync(employeeId, unreadOnly: true);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
           
            await _notificationRepository.UpdateRangeAsync(notifications);
        }

        private NotificationResponseDto MapToDto(Notification notification, string? employeeName) // Made employeeName nullable
        {
            return new NotificationResponseDto
            {
                Id = notification.Id,
                EmployeeId = notification.EmployeeId, // Now correctly maps Guid? to Guid?
                EmployeeName = employeeName ?? "Unknown", // Handle null employeeName
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}