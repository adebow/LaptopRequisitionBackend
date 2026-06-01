using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.OTP
{
    public class GenerateOtpRequest
    {
        [Required]
        public string UserReference { get; set; } = default!;
        public int Time { get; set; } = 5; // Default to 5 minutes
        public string OtpDigit { get; set; } = "SixDigits"; // Default to SixDigits
    }
}