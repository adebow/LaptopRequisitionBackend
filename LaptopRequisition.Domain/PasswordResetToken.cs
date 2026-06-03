using System;
using System.Collections.Generic;

namespace LaptopRequisition.Domain
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } 
        
        
        public Employee? Employee { get; set; }
    }
}