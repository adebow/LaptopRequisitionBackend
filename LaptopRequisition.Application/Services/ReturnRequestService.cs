using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using LaptopRequisition.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Services
{
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILaptopRepository _laptopRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public ReturnRequestService(
            IReturnRequestRepository returnRequestRepository,
            IEmployeeRepository employeeRepository,
            ILaptopRepository laptopRepository,
            IHttpContextAccessor httpContextAccessor,
            INotificationService notificationService,
            IEmailService emailService)
        {
            _returnRequestRepository = returnRequestRepository;
            _employeeRepository = employeeRepository;
            _laptopRepository = laptopRepository;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        private Guid GetCurrentEmployeeId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }
            return Guid.Parse(userId);
        }

        private async Task<ReturnRequestResponseDto> MapToDto(ReturnRequest returnRequest)
        {
            var employee = await _employeeRepository.GetByIdAsync(returnRequest.EmployeeId);
            var laptop = await _laptopRepository.GetByIdAsync(returnRequest.LaptopId);

            return new ReturnRequestResponseDto
            {
                Id = returnRequest.Id,
                EmployeeId = returnRequest.EmployeeId,
                EmployeeName = employee?.FullName,
                LaptopId = returnRequest.LaptopId,
                LaptopSerialNumber = laptop?.SerialNumber,
                Reason = returnRequest.Reason,
                Status = Enum.Parse<ReturnRequestStatus>(returnRequest.Status), 
                CreatedAt = returnRequest.CreatedAt,
                ReturnedAt = returnRequest.ReturnedAt,
                UpdatedAt = returnRequest.UpdatedAt
            };
        }

        public async Task<ReturnRequestResponseDto> CreateReturnRequestAsync(CreateReturnRequestDto dto)
        {
            var employeeId = GetCurrentEmployeeId();
            
            var laptop = await _laptopRepository.GetByIdAsync(dto.LaptopId);
            if (laptop == null || !laptop.IsAssigned || laptop.AssignedToEmployeeId != employeeId)
            {
                throw new InvalidOperationException("Laptop not found or not assigned to the current employee.");
            }
            
            var existingPendingReturn = await _returnRequestRepository.GetPendingReturnRequestByLaptopIdAsync(dto.LaptopId); 
            if (existingPendingReturn != null)
            {
                throw new InvalidOperationException("A pending return request already exists for this laptop.");
            }

            var returnRequest = new ReturnRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                LaptopId = dto.LaptopId,
                Reason = dto.Reason,
                Status = ReturnRequestStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _returnRequestRepository.AddAsync(returnRequest);
            
            await _notificationService.CreateNotificationAsync(employeeId, $"Your return request for laptop {laptop.SerialNumber} has been submitted and is pending review.");
            
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee != null)
            {
                var emailSubject = "Laptop Return Request Submitted";
                var emailMessage = $"Dear {employee.FullName},\n\nYour request to return laptop {laptop.SerialNumber} has been submitted successfully. We will notify you once it has been processed.\n\nBest regards,\nLRS Team";
                await _emailService.SendEmailAsync(employee.Email, emailSubject, emailMessage);
            }

            return await MapToDto(returnRequest);
        }

        public async Task<ReturnRequestResponseDto> GetReturnRequestByIdAsync(Guid id)
        {
            var returnRequest = await _returnRequestRepository.GetByIdAsync(id);
            if (returnRequest == null)
            {
                throw new InvalidOperationException("Return request not found.");
            }
            return await MapToDto(returnRequest);
        }

        public async Task<IEnumerable<ReturnRequestResponseDto>> GetEmployeeReturnRequestsAsync(Guid employeeId)
        {
            var returnRequests = await _returnRequestRepository.GetByEmployeeIdAsync(employeeId);
            var dtos = new List<ReturnRequestResponseDto>();
            foreach (var rr in returnRequests)
            {
                dtos.Add(await MapToDto(rr));
            }
            return dtos;
        }

        public async Task<IEnumerable<ReturnRequestResponseDto>> GetAllReturnRequestsAsync()
        {
            var returnRequests = await _returnRequestRepository.GetAllAsync();
            var dtos = new List<ReturnRequestResponseDto>();
            foreach (var rr in returnRequests)
            {
                dtos.Add(await MapToDto(rr));
            }
            return dtos;
        }

        public async Task ApproveReturnRequestAsync(Guid returnRequestId)
        {
            var returnRequest = await _returnRequestRepository.GetByIdAsync(returnRequestId);
            if (returnRequest == null)
            {
                throw new InvalidOperationException("Return request not found.");
            }
            if (returnRequest.Status != ReturnRequestStatus.Pending.ToString())
            {
                throw new InvalidOperationException("Only pending return requests can be approved.");
            }

            returnRequest.Status = ReturnRequestStatus.Approved.ToString();
            returnRequest.UpdatedAt = DateTime.UtcNow;
            await _returnRequestRepository.UpdateAsync(returnRequest);
            
            var laptop = await _laptopRepository.GetByIdAsync(returnRequest.LaptopId);
            if (laptop != null)
            {
                laptop.IsAssigned = false;
                laptop.AssignedToEmployeeId = null;
                await _laptopRepository.UpdateAsync(laptop);
            }
            
            await _notificationService.CreateNotificationAsync(returnRequest.EmployeeId, $"Your return request for laptop {laptop?.SerialNumber} has been approved.");
            
            var employee = await _employeeRepository.GetByIdAsync(returnRequest.EmployeeId);
            if (employee != null)
            {
                var emailSubject = "Laptop Return Request Approved";
                var emailMessage = $"Dear {employee.FullName},\n\nYour request to return laptop {laptop?.SerialNumber} has been approved. Please proceed with the physical return process.\n\nBest regards,\nLRS Team";
                await _emailService.SendEmailAsync(employee.Email, emailSubject, emailMessage);
            }
        }

        public async Task RejectReturnRequestAsync(Guid returnRequestId, string reason)
        {
            var returnRequest = await _returnRequestRepository.GetByIdAsync(returnRequestId);
            if (returnRequest == null)
            {
                throw new InvalidOperationException("Return request not found.");
            }
            if (returnRequest.Status != ReturnRequestStatus.Pending.ToString())
            {
                throw new InvalidOperationException("Only pending return requests can be rejected.");
            }

            returnRequest.Status = ReturnRequestStatus.Rejected.ToString();
            returnRequest.Reason = reason;
            returnRequest.UpdatedAt = DateTime.UtcNow;
            await _returnRequestRepository.UpdateAsync(returnRequest);

            
            await _notificationService.CreateNotificationAsync(returnRequest.EmployeeId, $"Your return request for laptop {returnRequest.LaptopId} has been rejected. Reason: {reason}");
            
            
            var employee = await _employeeRepository.GetByIdAsync(returnRequest.EmployeeId);
            if (employee != null)
            {
                var emailSubject = "Laptop Return Request Rejected";
                var emailMessage = $"Dear {employee.FullName},\n\nYour request to return laptop {returnRequest.LaptopId} has been rejected. Reason: {reason}\n\nBest regards,\nLRS Team";
                await _emailService.SendEmailAsync(employee.Email, emailSubject, emailMessage);
            }
        }

        public async Task DeleteReturnRequestAsync(Guid returnRequestId)
        {
            var returnRequest = await _returnRequestRepository.GetByIdAsync(returnRequestId);
            if (returnRequest == null)
            {
                throw new InvalidOperationException("Return request not found.");
            }
            await _returnRequestRepository.DeleteAsync(returnRequestId);
        }
    }
}