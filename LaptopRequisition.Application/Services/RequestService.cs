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
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService; 

        public RequestService(
            IRequestRepository requestRepository,
            IEmployeeRepository employeeRepository,
            ILaptopRepository laptopRepository,
            IHttpContextAccessor httpContextAccessor,
            INotificationService notificationService,
            IEmailService emailService) 
        {
            _requestRepository = requestRepository;
            _employeeRepository = employeeRepository;
            _laptopRepository = laptopRepository;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
            _emailService = emailService; 
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
            
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found for current user.");
            }
            
            await _notificationService.CreateNotificationAsync(employeeId, $"Your laptop request (ID: {request.Id.ToString().Substring(0, 8)}...) has been submitted successfully.");
            
            var emailSubject = "Laptop Request Submitted Successfully";
            var emailMessage = $"Dear {employee.FullName},\n\nYour laptop request with ID: {request.Id.ToString().Substring(0, 8)}... for '{request.Purpose}' has been submitted successfully and is now pending review.\n\nWe will notify you once there is an update.\n\nBest regards,\nLRS Team";
            await _emailService.SendEmailAsync(employee.Email, emailSubject, emailMessage);

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
            request.ApprovedRejectedAt = DateTime.UtcNow; 

            await _requestRepository.UpdateAsync(request);
            
            await _notificationService.CreateNotificationAsync(request.EmployeeId, $"Your laptop request (ID: {request.Id.ToString().Substring(0, 8)}...) has been approved!");
        }
        
        public async Task RejectRequestAsync(Guid requestId, string reason)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new InvalidOperationException("Request not found.");

            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be rejected.");

            request.Status = RequestStatus.Rejected;
            request.RejectionReason = reason; 
            request.UpdatedAt = DateTime.UtcNow;
            request.ApprovedRejectedAt = DateTime.UtcNow;

            await _requestRepository.UpdateAsync(request);
            
            await _notificationService.CreateNotificationAsync(request.EmployeeId, $"Your laptop request (ID: {request.Id.ToString().Substring(0, 8)}...) has been rejected. Reason: {reason}");
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
            request.AssignedAt = DateTime.UtcNow;

            laptop.IsAssigned = true;
            await _laptopRepository.UpdateAsync(laptop);

            await _requestRepository.UpdateAsync(request);
            
            await _notificationService.CreateNotificationAsync(request.EmployeeId, $"A laptop ({laptop.SerialNumber}) has been assigned to your request (ID: {request.Id.ToString().Substring(0, 8)}...). Please check your request status.");
        }
        
        private RequestResponseDto Map(Request request)
        {
           var employee = _employeeRepository.GetByIdAsync(request.EmployeeId).Result; 
            var laptop = request.LaptopId.HasValue ? _laptopRepository.GetByIdAsync(request.LaptopId.Value).Result : null;

            return new RequestResponseDto
            {
                Id = request.Id,
                EmployeeId = request.EmployeeId,
                EmployeeName = employee?.FullName,
                Status = request.Status, 
                Purpose = request.Purpose,
                PreferredSpecs = request.PreferredSpecs,
                IsSwapRequest = request.IsSwapRequest, 
                RejectionReason = request.RejectionReason, 
                LaptopId = request.LaptopId,
                LaptopName = laptop?.SerialNumber,
                IsReceiptConfirmed = request.IsReceiptConfirmed,
                CreatedAt = request.CreatedAt,
                ApprovedRejectedAt = request.ApprovedRejectedAt,
                AssignedAt = request.AssignedAt,
                ReceiptConfirmedAt = request.ReceiptConfirmedAt
            };
        }
    }
}