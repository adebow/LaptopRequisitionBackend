using LaptopRequisition.Application.DTOs.Admin;
using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaptopRequisition.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin/roles")] // Dedicated route for admin role management
    [Authorize(Roles = "Admin")] // Protects all endpoints in this controller
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet] // GET /api/admin/roles
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoleResponseDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while fetching roles.", details = ex.Message });
            }
        }

        [HttpGet("{id}")] // GET /api/admin/roles/{id}
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                return Ok(role);
            }
            catch (InvalidOperationException ex) // For "Role not found"
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while fetching the role.", details = ex.Message });
            }
        }

        [HttpPost] // POST /api/admin/roles
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoleResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newRole = await _roleService.CreateRoleAsync(dto);
                return CreatedAtAction(nameof(GetRoleById), new { id = newRole.Id }, newRole);
            }
            catch (InvalidOperationException ex) // For "Role with name already exists"
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while creating the role.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedRole = await _roleService.UpdateRoleAsync(id, dto);
                return Ok(updatedRole);
            }
            catch (InvalidOperationException ex) 
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while updating the role.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")] // DELETE /api/admin/roles/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (InvalidOperationException ex) // For "Role not found" or "assigned to employees"
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while deleting the role.", details = ex.Message });
            }
        }
    }
}