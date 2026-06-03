using System;
using System.Collections.Generic;
using LaptopRequisition.Application.DTOs.Notification; // Changed from .DTOs.Notification
using LaptopRequisition.Domain.Enums; // For RequestStatus


namespace LaptopRequisition.Application.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalRequests { get; set; }
        public CurrentLaptopDetailsDto? CurrentLaptop { get; set; }
        public RequestStatusSummaryDto? CurrentRequestStatus { get; set; }
        public int UnreadNotificationsCount { get; set; }
        public List<NotificationResponseDto> RecentNotifications { get; set; } = new List<NotificationResponseDto>(); // Changed to NotificationResponseDto
        public bool IsReturnRequestPending { get; set; } // Added this line
    }

    public class CurrentLaptopDetailsDto
    {
        public Guid Id { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string RAM { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string ScreenSize { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        // Add other relevant details as needed
    }

    public class RequestStatusSummaryDto
    {
        public Guid? RequestId { get; set; }
        public RequestStatus Status { get; set; }
        public string? Purpose { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool HasActiveRequest { get; set; }
        public bool IsReceiptConfirmationPending { get; set; } // For the "Confirm Receipt" button
    }
}