using LaptopRequisition.Application.DTOs.Employee;
using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaptopRequisition.WebAPI.Controllers
{
    [Authorize] // Secure the entire controller
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(IProfileService profileService, IHttpContextAccessor httpContextAccessor)
        {
            _profileService = profileService;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentEmployeeId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User not authenticated or employee ID not found in token.");
            }
            return Guid.Parse(userId);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                var profile = await _profileService.GetProfileAsync(employeeId);
                return Ok(profile);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while fetching profile.", details = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                await _profileService.UpdateProfileAsync(employeeId, dto);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while updating profile.", details = ex.Message });
            }
        }

        [HttpPost("picture")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // Returns URL of the uploaded picture
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadProfilePicture([FromForm] ProfilePictureUploadDto dto)
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                var imageUrl = await _profileService.UploadProfilePictureAsync(employeeId, dto.File);
                return Ok(new { imageUrl });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while uploading profile picture.", details = ex.Message });
            }
        }

        [HttpDelete("picture")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveProfilePicture()
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                await _profileService.RemoveProfilePictureAsync(employeeId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while removing profile picture.", details = ex.Message });
            }
        }
    }
}