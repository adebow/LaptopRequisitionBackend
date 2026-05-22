namespace LaptopRequisition.Domain
{
    public class Laptop
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Model name
        public string SerialNumber { get; set; } // unique
        public string Specifications { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime CreatedAt { get; set; } // Added for audit
        public DateTime UpdatedAt { get; set; } // Added for audit

        
        public ICollection<Request> Requests { get; set; }
        public ICollection<ReturnRequest> ReturnRequests { get; set; }
    }
}