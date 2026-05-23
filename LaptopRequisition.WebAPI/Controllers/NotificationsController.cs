using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace LaptopRequisition.WebAPI.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationsController(INotificationService notificationService, IHttpContextAccessor httpContextAccessor)
        {
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
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
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NotificationResponseDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetNotifications([FromQuery] bool unreadOnly = false)
        {
            var employeeId = GetCurrentEmployeeId();
            var notifications = await _notificationService.GetNotificationsByEmployeeIdAsync(employeeId, unreadOnly);
            return Ok(notifications);
        }
        
        [HttpGet("latest/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NotificationResponseDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetLatestNotifications(int count)
        {
            var employeeId = GetCurrentEmployeeId();
            var notifications = await _notificationService.GetLatestNotificationsByEmployeeIdAsync(employeeId, count);
            return Ok(notifications);
        }
        
        [HttpPut("{notificationId}/read")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
        {
            var employeeId = GetCurrentEmployeeId();
            var notification = await _notificationService.GetNotificationByIdAsync(notificationId);
            if (notification == null || notification.EmployeeId != employeeId)
            {
                return NotFound("Notification not found or does not belong to the current user.");
            }

            await _notificationService.MarkNotificationAsReadAsync(notificationId);
            return NoContent();
        }

       
        [HttpPut("read-all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            var employeeId = GetCurrentEmployeeId();
            await _notificationService.MarkAllNotificationsAsReadAsync(employeeId);
            return NoContent();
        }
    }
}