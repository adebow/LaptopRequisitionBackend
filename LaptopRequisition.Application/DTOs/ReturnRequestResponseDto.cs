using LaptopRequisition.Domain.Enums;
using System;

namespace LaptopRequisition.Application.DTOs
{
    public class ReturnRequestResponseDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } // To display employee name
        public Guid LaptopId { get; set; }
        public string LaptopSerialNumber { get; set; } // To display laptop details
        public string Reason { get; set; }
        public ReturnRequestStatus Status { get; set; } // Using enum for status
        public DateTime CreatedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}