using System;
using LaptopRequisition.Domain.Enums;

namespace LaptopRequisition.Application.DTOs.Request
{
    public class HistoryFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public RequestStatus? Status { get; set; }
        public string? RequestType { get; set; } // "LaptopRequest" or "ReturnRequest"
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}