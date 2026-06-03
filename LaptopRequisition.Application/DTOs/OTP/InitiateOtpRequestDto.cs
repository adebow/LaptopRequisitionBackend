using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.OTP
{
    public class InitiateOtpRequestDto
    {
        [Required]
        [EmailAddress]
        public string UserReference { get; set; } = default!;
    }
}