using System;

namespace LaptopRequisition.Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; } // Changed to nullable
        public string EmployeeName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}