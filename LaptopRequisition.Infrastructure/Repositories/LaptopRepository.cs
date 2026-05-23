using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using Microsoft.EntityFrameworkCore;


namespace LaptopRequisition.Infrastructure.Repositories
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly ApplicationDbContext _context;

        public LaptopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Laptop> GetByIdAsync(Guid id)
        {
            return await _context.Laptops.FindAsync(id);
        }

        public async Task<Laptop> GetBySerialNumberAsync(string serialNumber)
        {
            return await _context.Laptops.FirstOrDefaultAsync(l => l.SerialNumber == serialNumber);
        }

        public async Task<IEnumerable<Laptop>> GetAllAsync()
        {
            return await _context.Laptops.ToListAsync();
        }

        public async Task AddAsync(Laptop laptop)
        {
            await _context.Laptops.AddAsync(laptop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Laptop laptop)
        {
            _context.Laptops.Update(laptop);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
                await _context.SaveChangesAsync();
            }
        }
    }
}