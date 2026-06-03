using System;
using System.Collections.Generic; // For ICollection if needed for history/laptops
using LaptopRequisition.Application.DTOs.Request; // Added for RequestHistoryDto

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class AdminEmployeeProfileDto
    {
        public Guid Id { get; set; }
        public string StaffId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public bool IsVerified { get; set; }
        public bool IsFirstLogin { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Additional details for admin view
        public LaptopRequisition.Application.DTOs.Laptop.LaptopResponseDto? AssignedLaptop { get; set; } // Details of currently assigned laptop
        public IEnumerable<RequestHistoryDto>? RequestHistory { get; set; } // Summary of request history
    }
}