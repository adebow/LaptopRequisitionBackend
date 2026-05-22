using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface ILaptopRepository
    {
        Task<Laptop> GetByIdAsync(Guid id);
        Task<Laptop> GetBySerialNumberAsync(string serialNumber);
        Task<IEnumerable<Laptop>> GetAllAsync();
        Task AddAsync(Laptop laptop);
        Task UpdateAsync(Laptop laptop);
        Task DeleteAsync(Guid id);
    }
}