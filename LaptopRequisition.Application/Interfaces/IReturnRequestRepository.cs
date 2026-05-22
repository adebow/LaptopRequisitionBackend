using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IReturnRequestRepository
    {
        Task<ReturnRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<ReturnRequest>> GetByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<ReturnRequest>> GetAllAsync();
        Task AddAsync(ReturnRequest returnRequest);
        Task UpdateAsync(ReturnRequest returnRequest);
        Task DeleteAsync(Guid id);
    }
}