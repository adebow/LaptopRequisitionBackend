using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs
{
    public class CreateNotificationDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Message { get; set; }
    }
}