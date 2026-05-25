using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs
{
    public class CreateReturnRequestDto
    {
        [Required]
        public Guid LaptopId { get; set; } // The ID of the laptop being returned

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Reason { get; set; } // Reason for returning the laptop
    }
}