using LaptopRequisition.Domain;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Employee employee);
    }
}