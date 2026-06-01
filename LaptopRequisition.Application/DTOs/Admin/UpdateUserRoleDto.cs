using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class UpdateUserRoleDto
    {
        [Required]
        public Guid NewRoleId { get; set; }
    }
}