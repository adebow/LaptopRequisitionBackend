using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs;

public class AssignLaptopDto
{
     [Required] 
     public Guid LaptopId { get; set; }
}