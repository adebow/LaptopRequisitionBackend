using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs;

public class RejectRequestDto
{
    [Required]
    [StringLength(500)]
    public string Reason { get; set; }
}