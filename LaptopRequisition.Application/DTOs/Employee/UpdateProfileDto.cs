using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Employee
{
    public class UpdateProfileDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}