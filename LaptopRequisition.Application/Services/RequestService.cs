using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using LaptopRequisition.Domain.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace LaptopRequisition.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestRepository _requestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILaptopRepository _laptopRepository;

        public RequestService(
            IRequestRepository requestRepository,
            IEmployeeRepository employeeRepository,
            ILaptopRepository laptopRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _requestRepository = requestRepository;
            _employeeRepository = employeeRepository;
            _laptopRepository = laptopRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        private Guid GetCurrentEmployeeId()
        {
            var userId = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
            if (userId == null)
                throw new UnauthorizedAccessException("User not authenticated.");
        
            return Guid.Parse(userId);
        }
        
        public async Task<RequestResponseDto> CreateRequestAsync(CreateRequestDto dto)
        {
            var employeeId = GetCurrentEmployeeId();

            var existingPending = await _requestRepository.GetPendingRequestByEmployeeIdAsync(employeeId);
            if (existingPending != null)
                throw new InvalidOperationException("You already have a pending request.");

            var request = new Request
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,

                Purpose = dto.Purpose,
                PreferredSpecs = dto.PreferredSpecs,
                IsSwapRequest = dto.IsSwapRequest,

                Status = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _requestRepository.AddAsync(request);

            return Map(request);
        }
        
        public async Task<RequestResponseDto> GetRequestByIdAsync(Guid id)
        {
            var request = await _requestRepository.GetByIdAsync(id);

            if (request == null)
                throw new InvalidOperationException("Request not found.");

            return Map(request);
        }
        
        public async Task<IEnumerable<RequestResponseDto>> GetEmployeeRequestsAsync(Guid employeeId)
        {
            var requests = await _requestRepository.GetByEmployeeIdAsync(employeeId);

            return requests.Select(Map);
        }
        
        public async Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync()
        {
            var requests = await _requestRepository.GetAllAsync();

            return requests.Select(Map);
        }
        
        public async Task ApproveRequestAsync(Guid requestId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new InvalidOperationException("Request not found.");

            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be approved.");

            request.Status = RequestStatus.Approved;
            request.UpdatedAt = DateTime.UtcNow;
            request.ApprovedRejectedAt = DateTime.UtcNow; // Set ApprovedRejectedAt

            await _requestRepository.UpdateAsync(request);
        }
        
        public async Task RejectRequestAsync(Guid requestId, string reason)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new InvalidOperationException("Request not found.");

            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be rejected.");

            request.Status = RequestStatus.Rejected;
            request.RejectionReason = reason; // Set rejection reason
            request.UpdatedAt = DateTime.UtcNow;
            request.ApprovedRejectedAt = DateTime.UtcNow; // Set ApprovedRejectedAt

            await _requestRepository.UpdateAsync(request);
        }
        
        public async Task AssignLaptopAsync(Guid requestId, Guid laptopId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            var laptop = await _laptopRepository.GetByIdAsync(laptopId);

            if (request == null)
                throw new InvalidOperationException("Request not found.");

            if (laptop == null)
                throw new InvalidOperationException("Laptop not found.");

            if (request.Status != RequestStatus.Approved)
                throw new InvalidOperationException("Only approved requests can be assigned.");

            request.LaptopId = laptopId;
            request.Status = RequestStatus.Assigned;
            request.UpdatedAt = DateTime.UtcNow;
            request.AssignedAt = DateTime.UtcNow; // Set AssignedAt

            laptop.IsAssigned = true;
            await _laptopRepository.UpdateAsync(laptop);

            await _requestRepository.UpdateAsync(request);
        }
        
        private RequestResponseDto Map(Request request)
        {
            // Eager load Employee and Laptop if needed for EmployeeName and LaptopName
            // For now, these will be null if not loaded.
            // This might require changes in IRequestRepository.GetByIdAsync or GetAllAsync to include Employee/Laptop.
            var employee = _employeeRepository.GetByIdAsync(request.EmployeeId).Result; // Synchronous call for simplicity, consider async
            var laptop = request.LaptopId.HasValue ? _laptopRepository.GetByIdAsync(request.LaptopId.Value).Result : null; // Synchronous call for simplicity, consider async

            return new RequestResponseDto
            {
                Id = request.Id,
                EmployeeId = request.EmployeeId,
                EmployeeName = employee?.FullName, // Map EmployeeName
                Status = request.Status, // Corrected: Assign enum directly
                Purpose = request.Purpose,
                PreferredSpecs = request.PreferredSpecs,
                IsSwapRequest = request.IsSwapRequest, // Map IsSwapRequest
                RejectionReason = request.RejectionReason, // Map RejectionReason
                LaptopId = request.LaptopId,
                LaptopName = laptop?.SerialNumber, // Map LaptopName (assuming SerialNumber is a good name)
                IsReceiptConfirmed = request.IsReceiptConfirmed, // Map IsReceiptConfirmed
                CreatedAt = request.CreatedAt,
                ApprovedRejectedAt = request.ApprovedRejectedAt, // Map ApprovedRejectedAt
                AssignedAt = request.AssignedAt, // Map AssignedAt
                ReceiptConfirmedAt = request.ReceiptConfirmedAt // Map ReceiptConfirmedAt
            };
        }
    }
}