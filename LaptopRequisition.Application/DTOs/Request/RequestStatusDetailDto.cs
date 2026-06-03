using System;
using System.Collections.Generic;
using LaptopRequisition.Domain.Enums;

namespace LaptopRequisition.Application.DTOs.Request
{
    public class RequestStatusDetailDto
    {
        public Guid RequestId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string? RequestedLaptopModel { get; set; } // From PreferredSpecs or initial request
        public RequestStatus CurrentStatus { get; set; }
        public string? Purpose { get; set; }
        public string? PreferredSpecs { get; set; }
        public string? RejectionReason { get; set; }
        public bool IsReceiptConfirmed { get; set; }
        public DateTime? ReceiptConfirmedAt { get; set; }

        public AssignedLaptopDetailDto? AssignedLaptop { get; set; }
        public List<RequestTimelineEventDto> Timeline { get; set; } = new List<RequestTimelineEventDto>();
        public bool IsDismissed { get; set; } // For dismissing rejected requests
        public bool HasActiveRequest { get; set; } // Added this property
    }

    public class AssignedLaptopDetailDto
    {
        public Guid Id { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string RAM { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty; // Enum converted to string
        public string ScreenSize { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
    }

    public class RequestTimelineEventDto
    {
        public RequestStatus Status { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Notes { get; set; } // e.g., rejection reason, alternative device info
    }
}