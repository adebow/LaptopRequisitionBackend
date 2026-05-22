using LaptopRequisition.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
        Task<DepartmentResponseDto> GetDepartmentByIdAsync(Guid id);
        Task<IEnumerable<DepartmentResponseDto>> GetAllDepartmentsAsync();
        Task<DepartmentResponseDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto updateDepartmentDto);
        Task DeleteDepartmentAsync(Guid id);
    }
}