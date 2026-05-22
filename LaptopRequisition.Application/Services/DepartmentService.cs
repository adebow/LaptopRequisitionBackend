using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var existingDepartment = await _departmentRepository.GetByNameAsync(createDepartmentDto.Name);
        if (existingDepartment != null)
        {
            throw new InvalidOperationException($"Department with name '{createDepartmentDto.Name}' already exists.");
        }

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = createDepartmentDto.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _departmentRepository.AddAsync(department);
        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }

    public async Task<DepartmentResponseDto> GetDepartmentByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with ID '{id}' not found.");
        }

        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }

    public async Task<IEnumerable<DepartmentResponseDto>> GetAllDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(department => new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        }).ToList();
    }

    public async Task<DepartmentResponseDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto updateDepartmentDto)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with ID '{id}' not found.");
        }

        var existingDepartmentWithName = await _departmentRepository.GetByNameAsync(updateDepartmentDto.Name);
        if (existingDepartmentWithName != null && existingDepartmentWithName.Id != id)
        {
            throw new InvalidOperationException($"Department with name '{updateDepartmentDto.Name}' already exists.");
        }

        department.Name = updateDepartmentDto.Name;
        department.UpdatedAt = DateTime.UtcNow;

        await _departmentRepository.UpdateAsync(department);
        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }

    public async Task DeleteDepartmentAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with ID '{id}' not found.");
        }
       
        await _departmentRepository.DeleteAsync(id);
    }
}