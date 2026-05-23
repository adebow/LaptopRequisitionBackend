using LaptopRequisition.Domain;


namespace LaptopRequisition.Application.Interfaces
{
    public interface IRequestRepository
    {
        Task AddAsync(Request request);

        Task<Request?> GetByIdAsync(Guid id);

        Task<IEnumerable<Request>> GetAllAsync();

        Task<IEnumerable<Request>> GetByEmployeeIdAsync(Guid employeeId);

        Task<Request?> GetPendingRequestByEmployeeIdAsync(Guid employeeId);

        Task UpdateAsync(Request request);

        Task DeleteAsync(Guid id);

    }
}