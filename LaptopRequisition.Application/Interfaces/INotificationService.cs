using LaptopRequisition.Application.DTOs.Notification; // Added for DTOs
using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(Guid employeeId, string message);
        Task<IEnumerable<NotificationResponseDto>> GetNotificationsByEmployeeIdAsync(Guid employeeId, bool unreadOnly = false);
        Task<IEnumerable<NotificationResponseDto>> GetRecentNotificationsByEmployeeIdAsync(Guid employeeId, int count); // Renamed
        Task<NotificationResponseDto> GetNotificationByIdAsync(Guid notificationId); // Added for controller security check
        Task MarkNotificationAsReadAsync(Guid notificationId);
        Task MarkAllNotificationsAsReadAsync(Guid employeeId);
    }
}