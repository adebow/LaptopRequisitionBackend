using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
    }
}