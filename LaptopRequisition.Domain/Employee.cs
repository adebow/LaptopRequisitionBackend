namespace LaptopRequisition.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string StaffId { get; set; } 
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid DepartmentId { get; set; } 
        public String Role { get; set; }       
        public string PasswordHash { get; set; }
        public int FailedLoginCount { get; set; }
        public bool IsLocked { get; set; }
        public string PreviousPasswordHashes { get; set; } = "[]"; 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

      
        public Department Department { get; set; } 

        public ICollection<Request> Requests { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<ReturnRequest> ReturnRequests { get; set; }
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}