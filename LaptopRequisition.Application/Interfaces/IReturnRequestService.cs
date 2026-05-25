using LaptopRequisition.Application.DTOs;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IReturnRequestService
    {
        Task<ReturnRequestResponseDto> CreateReturnRequestAsync(CreateReturnRequestDto dto);
        Task<ReturnRequestResponseDto> GetReturnRequestByIdAsync(Guid id);
        Task<IEnumerable<ReturnRequestResponseDto>> GetEmployeeReturnRequestsAsync(Guid employeeId);
        Task<IEnumerable<ReturnRequestResponseDto>> GetAllReturnRequestsAsync();
        Task ApproveReturnRequestAsync(Guid returnRequestId);
        Task RejectReturnRequestAsync(Guid returnRequestId, string reason);
        Task DeleteReturnRequestAsync(Guid returnRequestId); 
    }
}