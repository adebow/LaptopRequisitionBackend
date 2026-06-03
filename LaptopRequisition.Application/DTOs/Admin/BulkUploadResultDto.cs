namespace LaptopRequisition.Application.DTOs.Admin
{
    public class BulkUploadResultDto
    {
        public string StaffId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}