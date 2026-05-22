using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LaptopRequisition.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto dto)
        {
            var result = await _departmentService.CreateDepartmentAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _departmentService.GetAllDepartmentsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, UpdateDepartmentDto dto)
        {
            var result = await _departmentService.UpdateDepartmentAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return NoContent();
        }
    }
}