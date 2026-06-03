using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Employee
{
    public class ProfilePictureUploadDto
    {
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; } = default!; // Represents the uploaded file
    }
}