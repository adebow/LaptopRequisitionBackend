using LaptopRequisition.Application.DTOs;

namespace LaptopRequisition.Application.Interfaces;

public interface ILaptopService
{
    Task<LaptopResponseDto> CreateLaptopAsync(CreateLaptopDto dto);
    Task<IEnumerable<LaptopResponseDto>> GetAllLaptopsAsync();
    Task<LaptopResponseDto> GetLaptopByIdAsync(Guid id);
    Task<LaptopResponseDto> UpdateLaptopAsync(Guid id, UpdateLaptopDto dto);
    Task DeleteLaptopAsync(Guid id);
}