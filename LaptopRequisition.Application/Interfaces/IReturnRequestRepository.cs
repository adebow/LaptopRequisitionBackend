using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IReturnRequestRepository
    {
        Task AddAsync(ReturnRequest returnRequest);
        Task UpdateAsync(ReturnRequest returnRequest);
        Task<ReturnRequest?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReturnRequest>> GetByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<ReturnRequest>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<ReturnRequest?> GetPendingReturnRequestByLaptopIdAsync(Guid laptopId); // Added this line
    }
}