
namespace LaptopRequisition.Domain
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        
        public ICollection<Employee> Employees { get; set; }
    }
}