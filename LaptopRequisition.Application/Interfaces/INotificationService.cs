using LaptopRequisition.Application.DTOs; // Added for DTOs
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
        Task<IEnumerable<NotificationResponseDto>> GetLatestNotificationsByEmployeeIdAsync(Guid employeeId, int count); // Added for dashboard
        Task<NotificationResponseDto> GetNotificationByIdAsync(Guid notificationId); // Added for controller security check
        Task MarkNotificationAsReadAsync(Guid notificationId);
        Task MarkAllNotificationsAsReadAsync(Guid employeeId);
    }
}