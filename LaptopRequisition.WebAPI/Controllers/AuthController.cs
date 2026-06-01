using LaptopRequisition.Application.DTOs.Login;

using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using System;
using LaptopRequisition.Application.DTOs;

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
        [ProducesResponseType(StatusCodes.Status202Accepted)] // Changed to 202 Accepted
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employee = await _authService.RegisterEmployeeAsync(registerDto);
                // Return 202 Accepted, indicating further action (OTP verification) is needed
                return StatusCode(StatusCodes.Status202Accepted, new
                {
                    Message = "Employee registered successfully. Please verify your account with OTP.",
                    EmployeeId = employee.Id,
                    ValidationReference = registerDto.ValidationReference // Return validation reference for frontend to use in OTP verification
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, // Use StatusCodes
                    new { Message = "An error occurred during registration.", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var loginResponse = await _authService.LoginAsync(loginDto.Email, loginDto.Password); // Changed to LoginResponseDto
                return Ok(loginResponse); // Return the full LoginResponseDto
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred during login.", Details = ex.Message });
            }
        }

        [HttpPost("request-password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred during password reset request.", Details = ex.Message });
            }
        }

        // Uncommented and added ProducesResponseType for ResetPassword
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.ResetPasswordAsync(resetDto.Token, resetDto.NewPassword);
                return Ok(new { Message = "Password has been reset successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred during password reset.", Details = ex.Message });
            }
        }
    }
}