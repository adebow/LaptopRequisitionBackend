using LaptopRequisition.Application.DTOs;


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