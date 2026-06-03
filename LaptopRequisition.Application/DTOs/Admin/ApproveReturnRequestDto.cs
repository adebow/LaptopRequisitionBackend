using System;
using System.ComponentModel.DataAnnotations;
using LaptopRequisition.Domain.Enums; // Added for LaptopStatus

namespace LaptopRequisition.Application.DTOs.Admin
{
    public class ApproveReturnRequestDto
    {
        [Required]
        public Guid ReturnRequestId { get; set; }

        [Required]
        public LaptopStatus ReturnedCondition { get; set; } // Good, Damaged, Needs Repair (maps to Available, UnderRepair)
    }
}