using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class BulkRegisterEmployeeDto
    {
        [Required]
        [StringLength(50)]
        public string StaffId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty; // Admin will provide department name

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty; // Admin will provide role name

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty; // Initial password for SSO
    }
}