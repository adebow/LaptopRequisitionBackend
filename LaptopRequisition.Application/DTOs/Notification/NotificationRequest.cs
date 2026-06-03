using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Application.DTOs.Notification
{
    public class NotificationRequest
    {
        public List<string> Channels { get; set; } = []; // e.g., "Email", "SMS"
        [Required]
        public string From { get; set; } = string.Empty;
        [Required]
        public string To { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}