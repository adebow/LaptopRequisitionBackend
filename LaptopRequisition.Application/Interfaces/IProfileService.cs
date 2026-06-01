using LaptopRequisition.Application.DTOs.Employee;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Added for IFormFile

namespace LaptopRequisition.Application.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileAsync(Guid employeeId);
        Task UpdateProfileAsync(Guid employeeId, UpdateProfileDto dto);
        Task<string> UploadProfilePictureAsync(Guid employeeId, IFormFile file); // Returns URL or path
        Task RemoveProfilePictureAsync(Guid employeeId);
    }
}