using System;

namespace LaptopRequisition.Application.DTOs.Admin.Reports
{
    public class RequestTrendReportDto
    {
        public DateTime Date { get; set; }
        public int NewRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int CompletedRequests { get; set; }
    }
}