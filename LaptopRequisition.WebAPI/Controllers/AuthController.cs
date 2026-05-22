using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LaptopRequisition.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employee = await _authService.RegisterEmployeeAsync(registerDto);
                return StatusCode(201, new { Message = "Employee registered successfully", EmployeeId = employee.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logger)
                return StatusCode(500,
                    new { Message = "An error occurred during registration.", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "An error occurred during login.", Details = ex.Message });
            }
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.RequestPasswordResetAsync(requestDto.Email);
                return Ok(new
                    { Message = "If an account with that email exists, a password reset link has been sent." });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500,
                    new { Message = "An error occurred during password reset request.", Details = ex.Message });
            }
        }
    }
}

//         [HttpPost("reset-password")]
//         public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return BadRequest(ModelState);
//             }
//
//             try
//             {
//                 await _authService.ResetPasswordAsync(resetDto.Token, resetDto.NewPassword);
//                 return Ok(new { Message = "Password has been reset successfully." });
//             }
//             catch (InvalidOperationException ex)
//             {
//                 return BadRequest(new { Message = ex.Message });
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception
//                 return StatusCode(500, new { Message = "An error occurred during password reset.", Details = ex.Message });
//             }
//         }
//     }
// }