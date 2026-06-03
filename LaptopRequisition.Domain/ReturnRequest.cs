using System;
using System.Collections.Generic;

namespace LaptopRequisition.Domain
{
    public class ReturnRequest
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid LaptopId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 

        
        public Employee? Employee { get; set; }
        public Laptop Laptop { get; set; } = null!;
    }
}