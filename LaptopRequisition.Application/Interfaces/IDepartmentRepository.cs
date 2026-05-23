using LaptopRequisition.Domain;


namespace LaptopRequisition.Application.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Department> GetByIdAsync(Guid id);
        Task<Department> GetByNameAsync(string name);
        Task<IEnumerable<Department>> GetAllAsync();
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(Guid id);
    }
}