using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}