using LaptopRequisition.Domain.Enums;

namespace LaptopRequisition.Application.DTOs;

public class RequestResponseDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; }

    public RequestStatus Status { get; set; }

    public string Purpose { get; set; }

    public string? PreferredSpecs { get; set; }

    public bool IsSwapRequest { get; set; }

    public string? RejectionReason { get; set; }

    public Guid? LaptopId { get; set; }

    public string? LaptopName { get; set; }

    public bool IsReceiptConfirmed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ApprovedRejectedAt { get; set; }

    public DateTime? AssignedAt { get; set; }

    public DateTime? ReceiptConfirmedAt { get; set; }
}