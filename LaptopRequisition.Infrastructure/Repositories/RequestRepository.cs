using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using Microsoft.EntityFrameworkCore;
using LaptopRequisition.Domain.Enums;

namespace LaptopRequisition.Infrastructure.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Request request)
        {
            await _context.Requests.AddAsync(request);

            await _context.SaveChangesAsync();
        }

        public async Task<Request?> GetByIdAsync(Guid id)
        {
            return await _context.Requests
                .Include(r => r.Employee)
                .Include(r => r.Laptop)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Request>> GetAllAsync()
        {
            return await _context.Requests
                .Include(r => r.Employee)
                .Include(r => r.Laptop)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.Requests
                .Include(r => r.Employee)
                .Include(r => r.Laptop)
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Request?> GetPendingRequestByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.Requests
                .FirstOrDefaultAsync(r =>
                    r.EmployeeId == employeeId &&
                    r.Status == RequestStatus.Pending);
        }

        public async Task UpdateAsync(Request request)
        {
            _context.Requests.Update(request);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request != null)
            {
                _context.Requests.Remove(request);

                await _context.SaveChangesAsync();
            }
        }
    }
}