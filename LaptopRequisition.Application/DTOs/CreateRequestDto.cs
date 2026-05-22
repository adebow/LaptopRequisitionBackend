using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs
{
    public class CreateRequestDto
    {
               public bool IsSwapRequest { get; set; } = false;
       
               [Required]
               [StringLength(500, MinimumLength = 10)] 
               public string Purpose { get; set; }
       
               [StringLength(500)]
               public string? PreferredSpecs { get; set; }
               
    }
}