using LaptopRequisition.Domain.Enums;

namespace LaptopRequisition.Domain
{
    public class Request
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid? LaptopId { get; set; }

        public bool IsSwapRequest { get; set; }

        public RequestStatus Status { get; set; }

        public string Purpose { get; set; }

        public string PreferredSpecs { get; set; }

        public string? RejectionReason { get; set; }

        public bool IsReceiptConfirmed { get; set; }

        public DateTime? ReceiptConfirmedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? ApprovedRejectedAt { get; set; }

        public DateTime? AssignedAt { get; set; }

        public Employee Employee { get; set; }

        public Laptop? Laptop { get; set; }
    }
}