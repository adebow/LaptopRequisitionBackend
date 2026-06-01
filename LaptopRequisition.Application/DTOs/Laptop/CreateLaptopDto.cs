using System.ComponentModel.DataAnnotations;
using LaptopRequisition.Domain.Enums; // Added for OperatingSystemEnum and LaptopStatus
using System; // Added for DateTime

namespace LaptopRequisition.Application.DTOs.Laptop; // Updated namespace

public class CreateLaptopDto
{
    [Required]
    public string AssetTag { get; set; } = string.Empty;

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    [Required]
    public string SerialNumber { get; set; } = string.Empty;

    [Required]
    public string Processor { get; set; } = string.Empty;

    [Required]
    public string RAM { get; set; } = string.Empty;

    [Required]
    public string Storage { get; set; } = string.Empty;

    [Required]
    public OperatingSystemEnum OperatingSystem { get; set; }

    [Required]
    public string ScreenSize { get; set; } = string.Empty;

    // New Status property
    public LaptopStatus Status { get; set; } = LaptopStatus.Available;

    [Required]
    public DateTime PurchaseDate { get; set; } // Added

    [Required]
    public DateTime WarrantyExpiryDate { get; set; } // Added
}