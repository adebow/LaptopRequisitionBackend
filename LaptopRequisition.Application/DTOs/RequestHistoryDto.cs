using System;
using LaptopRequisition.Domain.Enums;

namespace LaptopRequisition.Application.DTOs
{
    public class RequestHistoryDto
    {
        public Guid RequestId { get; set; }

        public string RequestType { get; set; }

        public RequestStatus Status { get; set; }

        public string? LaptopName { get; set; }

        public DateTime Date { get; set; }

        public string Purpose { get; set; }
        
    }
}