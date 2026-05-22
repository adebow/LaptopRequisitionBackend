using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaptopRequisition.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRequest(CreateRequestDto dto)
        {
            var result = await _requestService.CreateRequestAsync(dto);

            return Ok(new
            {
                message = "Request submitted successfully",
                data = result
            });
        }

        
        [HttpGet("my-requests")]
        [Authorize]
        public async Task<IActionResult> GetMyRequests()
        {
            var employeeId =
                Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var requests =
                await _requestService.GetEmployeeRequestsAsync(employeeId);

            return Ok(requests);
        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _requestService.GetAllRequestsAsync();

            return Ok(requests);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);

            return Ok(request);
        }
        
        [HttpPut("{id}/approve")]
        [Authorize]
        public async Task<IActionResult> ApproveRequest(Guid id)
        {
            await _requestService.ApproveRequestAsync(id);

            return Ok(new
            {
                message = "Request approved successfully"
            });
        }
        
        [HttpPut("{id}/reject")]
        [Authorize]
        public async Task<IActionResult> RejectRequest(
            Guid id,
            RejectRequestDto dto)
        {
            await _requestService.RejectRequestAsync(id, dto.Reason);

            return Ok(new
            {
                message = "Request rejected successfully"
            });
        }

       
        [HttpPut("{id}/assign")]
        [Authorize]
        public async Task<IActionResult> AssignLaptop(
            Guid id,
            AssignLaptopDto dto)
        {
            await _requestService.AssignLaptopAsync(id, dto.LaptopId);

            return Ok(new
            {
                message = "Laptop assigned successfully"
            });
        }
    }
}