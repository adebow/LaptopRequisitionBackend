namespace LaptopRequisition.Application.DTOs.Admin
{
    public class AdminDashboardSummaryDto
    {
        public int TotalStaff { get; set; }
        public int TotalLaptops { get; set; }
        public int AvailableLaptops { get; set; }
        public int PendingRequests { get; set; }
    }
}