using LaptopRequisition.Domain;


namespace LaptopRequisition.Application.Interfaces
{
    public interface ILaptopRepository
    {
        Task<Laptop> GetByIdAsync(Guid id);
        Task<Laptop> GetBySerialNumberAsync(string serialNumber);
        Task<IEnumerable<Laptop>> GetAllAsync();
        Task AddAsync(Laptop laptop);
        Task UpdateAsync(Laptop laptop);
        Task DeleteAsync(Guid id);
    }
}