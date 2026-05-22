namespace LaptopRequisition.Domain
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Employee Employee { get; set; }
    }
}