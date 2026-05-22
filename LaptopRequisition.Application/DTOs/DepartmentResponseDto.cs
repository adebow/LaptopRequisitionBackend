using System;

namespace LaptopRequisition.Application.DTOs
{
    public class DepartmentResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}