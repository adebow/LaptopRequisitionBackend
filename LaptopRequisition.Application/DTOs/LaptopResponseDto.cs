namespace LaptopRequisition.Application.DTOs;

public class LaptopResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public string Specifications { get; set; }
    public bool IsActive { get; set; }
    public bool IsAssigned { get; set; }
}