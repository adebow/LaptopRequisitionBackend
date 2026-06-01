using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class AdminCreateEmployeeDto
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
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        public bool IsVerified { get; set; } = false; // Admin can choose to verify immediately
        public bool IsLocked { get; set; } = false; // Admin can choose to lock immediately
    }
}