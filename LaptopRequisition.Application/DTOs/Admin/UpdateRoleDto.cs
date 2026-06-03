using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class UpdateRoleDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}