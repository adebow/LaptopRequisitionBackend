using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class AssignLaptopToEmployeeDto
    {
        [Required]
        public Guid LaptopId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
    }
}