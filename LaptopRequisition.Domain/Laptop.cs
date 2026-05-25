namespace LaptopRequisition.Domain
{
    public class Laptop
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string SerialNumber { get; set; } 
        public string Specifications { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; }
        
        public Guid? AssignedToEmployeeId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 

        
        public ICollection<Request> Requests { get; set; }
        public ICollection<ReturnRequest> ReturnRequests { get; set; }
    }
}