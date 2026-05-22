using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        // Removed: [Required]
        // Removed: [Compare("NewPassword")]
        // Removed: public string ConfirmNewPassword { get; set; }
    }
}