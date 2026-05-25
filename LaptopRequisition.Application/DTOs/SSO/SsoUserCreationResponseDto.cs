namespace LaptopRequisition.Application.DTOs.SSO
{
    public class SsoUserCreationResponseDto
    {
        public bool IsSuccess { get; set; }
        public object? Data { get; set; } // Data might be null for successful creation
        public string? Message { get; set; }
    }
}