using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.OTP
{
    public class ValidateOtpRequest
    {
        [Required]
        public string RetrievalCode { get; set; } = default!;
        [Required]
        public string Otp { get; set; } = default!;
    }
}