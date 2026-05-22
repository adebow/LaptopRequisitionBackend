using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;

namespace LaptopRequisition.Application.Services;

public class LaptopService :ILaptopService
{
    private readonly ILaptopRepository _laptopRepository;
    
        public LaptopService(ILaptopRepository laptopRepository)
        {
            _laptopRepository = laptopRepository;
        }

    public async Task<LaptopResponseDto> CreateLaptopAsync(CreateLaptopDto dto)
    {
        var existingLaptop = await _laptopRepository
            .GetBySerialNumberAsync(dto.SerialNumber);
    
        if (existingLaptop != null)
        {
            throw new InvalidOperationException(
                $"Laptop with serial number '{dto.SerialNumber}' already exists.");
        }
    
        var laptop = new Laptop
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SerialNumber = dto.SerialNumber,
            Specifications = dto.Specifications,
            IsActive = true,
            IsAssigned = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    
        await _laptopRepository.AddAsync(laptop);
    
        return new LaptopResponseDto
        {
            Id = laptop.Id,
            Name = laptop.Name,
            SerialNumber = laptop.SerialNumber,
            Specifications = laptop.Specifications,
            IsActive = laptop.IsActive,
            IsAssigned = laptop.IsAssigned
        };
    }

    public async Task<IEnumerable<LaptopResponseDto>> GetAllLaptopsAsync()
    {
        var laptops = await _laptopRepository.GetAllAsync();

        return laptops.Select(l => new LaptopResponseDto
        {
            Id = l.Id,
            Name = l.Name,
            SerialNumber = l.SerialNumber,
            Specifications = l.Specifications,
            IsActive = l.IsActive,
            IsAssigned = l.IsAssigned
        });
    }

    public async Task<LaptopResponseDto> GetLaptopByIdAsync(Guid id)
    {
        var laptop = await _laptopRepository.GetByIdAsync(id);

        if (laptop == null)
        {
            throw new InvalidOperationException("Laptop not found.");
        }

        return new LaptopResponseDto
        {
            Id = laptop.Id,
            Name = laptop.Name,
            SerialNumber = laptop.SerialNumber,
            Specifications = laptop.Specifications,
            IsActive = laptop.IsActive,
            IsAssigned = laptop.IsAssigned
        };
    }

    public async Task<LaptopResponseDto> UpdateLaptopAsync(Guid id, UpdateLaptopDto dto)
    {
        var laptop = await _laptopRepository.GetByIdAsync(id);

        if (laptop == null)
        {
            throw new InvalidOperationException("Laptop not found.");
        }

        laptop.Name = dto.Name;
        laptop.Specifications = dto.Specifications;
        laptop.IsActive = dto.IsActive;
        laptop.UpdatedAt = DateTime.UtcNow;

        await _laptopRepository.UpdateAsync(laptop);

        return new LaptopResponseDto
        {
            Id = laptop.Id,
            Name = laptop.Name,
            SerialNumber = laptop.SerialNumber,
            Specifications = laptop.Specifications,
            IsActive = laptop.IsActive,
            IsAssigned = laptop.IsAssigned
        };
    }

    public async Task DeleteLaptopAsync(Guid id)
    {
        var laptop = await _laptopRepository.GetByIdAsync(id);
    
        if (laptop == null)
        {
            throw new InvalidOperationException("Laptop not found.");
        }
    
        if (laptop.IsAssigned)
        {
            throw new InvalidOperationException(
                "Cannot delete an assigned laptop.");
        }
    
        await _laptopRepository.DeleteAsync(id);
    }
}