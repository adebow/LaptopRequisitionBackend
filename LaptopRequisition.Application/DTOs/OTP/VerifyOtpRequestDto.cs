using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.OTP
{
    public class VerifyOtpRequestDto
    {
        [Required]
        public string ValidationReference { get; set; } = default!;
        [Required]
        public string Otp { get; set; } = default!;
    }
}