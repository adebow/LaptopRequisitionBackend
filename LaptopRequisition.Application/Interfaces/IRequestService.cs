using LaptopRequisition.Application.DTOs;


namespace LaptopRequisition.Application.Interfaces
{
    public interface IRequestService
    {
        Task<RequestResponseDto> CreateRequestAsync(CreateRequestDto dto);

        Task<RequestResponseDto> GetRequestByIdAsync(Guid id);

        Task<IEnumerable<RequestResponseDto>> GetEmployeeRequestsAsync(Guid employeeId);

        Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync();

        Task ApproveRequestAsync(Guid requestId);

        Task RejectRequestAsync(Guid requestId, string reason);

        Task AssignLaptopAsync(Guid requestId, Guid laptopId);
    }
}