using System;
using System.Collections.Generic;

namespace LaptopRequisition.Domain
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public Employee? Employee { get; set; }
    }
}