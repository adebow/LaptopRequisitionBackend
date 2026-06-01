using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Request
{
    public class ReportIssueDto
    {
        [Required]
        public Guid LaptopId { get; set; } // The ID of the laptop with the issue
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty; // Detailed description of the issue
        public string? ContactPreference { get; set; } // e.g., "Email", "Phone", "Teams"
    }
}