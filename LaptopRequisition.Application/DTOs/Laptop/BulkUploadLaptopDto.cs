using System.ComponentModel.DataAnnotations;
using LaptopRequisition.Domain.Enums; // Added for OperatingSystemEnum and LaptopStatus

namespace LaptopRequisition.Application.DTOs.Laptop
{
    public class BulkUploadLaptopDto
    {
        [Required]
        [StringLength(50)]
        public string AssetTag { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Processor { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string RAM { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Storage { get; set; } = string.Empty;

        [Required]
        public OperatingSystemEnum OperatingSystem { get; set; }

        [Required]
        [StringLength(20)]
        public string ScreenSize { get; set; } = string.Empty;

        public LaptopStatus Status { get; set; } = LaptopStatus.Available; // Default to Available
    }
}