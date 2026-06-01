using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class UpdateEmployeeDto
    {
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
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        public bool IsLocked { get; set; }
        public bool IsVerified { get; set; }
        public bool IsFirstLogin { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}