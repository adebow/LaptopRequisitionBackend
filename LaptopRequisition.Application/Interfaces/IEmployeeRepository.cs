using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(Guid id);
        Task<Employee> GetByStaffIdAsync(string staffId);
        Task<Employee> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);
    }
}