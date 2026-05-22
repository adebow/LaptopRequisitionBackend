using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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