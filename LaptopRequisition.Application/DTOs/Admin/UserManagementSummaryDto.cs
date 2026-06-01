namespace LaptopRequisition.Application.DTOs.Admin
{
    public class UserManagementSummaryDto
    {
        public int TotalStaff { get; set; }
        public int ActiveUsers { get; set; }
        public int PendingOnboarding { get; set; }
        public int UsersWithAssignedLaptops { get; set; }
        public int UsersWithoutLaptops { get; set; }
    }
}