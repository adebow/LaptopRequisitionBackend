using System.ComponentModel.DataAnnotations;
using System; // Added for Guid

namespace LaptopRequisition.Application.DTOs
{
    public class RegisterEmployeeDto
    {
        [Required]
        public string StaffId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Guid DepartmentId { get; set; } // Changed from String Department to Guid DepartmentId


        [Required]
        public String Role { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}