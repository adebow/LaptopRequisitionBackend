using System;
using LaptopRequisition.Domain.Enums; // Added for OperatingSystemEnum and LaptopStatus

namespace LaptopRequisition.Application.DTOs.Laptop; // Updated namespace

public class LaptopResponseDto
{
    public Guid Id { get; set; }
    public string AssetTag { get; set; } = string.Empty; // Added
    public string Brand { get; set; } = string.Empty;    // Added
    public string Model { get; set; } = string.Empty;    // Added
    public string SerialNumber { get; set; } = string.Empty;
    public string Processor { get; set; } = string.Empty; // Added
    public string RAM { get; set; } = string.Empty;      // Added
    public string Storage { get; set; } = string.Empty;   // Added
    public OperatingSystemEnum OperatingSystem { get; set; } // Added
    public string ScreenSize { get; set; } = string.Empty; // Added
    public LaptopStatus Status { get; set; } // Added new Status property
    public Guid? AssignedToEmployeeId { get; set; } // Added
    public string? AssignedToEmployeeName { get; set; } // Added for employee's full name
    public DateTime? AssignedAt { get; set; } // Added

    public DateTime PurchaseDate { get; set; } // Added
    public DateTime WarrantyExpiryDate { get; set; } // Added
}