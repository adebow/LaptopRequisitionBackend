using System;

namespace LaptopRequisition.Application.DTOs.Admin.Reports
{
    public class EmployeeActivityReportDto
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string StaffId { get; set; } = string.Empty;
        public int TotalRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int TotalReturnRequests { get; set; }
        public int ApprovedReturnRequests { get; set; }
        public int RejectedReturnRequests { get; set; }
        public int AssignedLaptopsCount { get; set; }
    }
}