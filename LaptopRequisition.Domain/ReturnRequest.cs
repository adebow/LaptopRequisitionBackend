namespace LaptopRequisition.Domain
{
    public class ReturnRequest
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid LaptopId { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 

        
        public Employee Employee { get; set; }
        public Laptop Laptop { get; set; }
    }
}