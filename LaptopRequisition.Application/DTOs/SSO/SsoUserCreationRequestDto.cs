using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.SSO
{
    public class SsoUserCreationRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string SourceId { get; set; }
    }
}