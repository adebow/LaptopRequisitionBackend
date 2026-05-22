using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRequisition.Infrastructure.Repositories
{
    public class ReturnRequestRepository : IReturnRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ReturnRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReturnRequest> GetByIdAsync(Guid id)
        {
            return await _context.ReturnRequests
                                 .Include(rr => rr.Employee)
                                 .Include(rr => rr.Laptop)
                                 .FirstOrDefaultAsync(rr => rr.Id == id);
        }

        public async Task<IEnumerable<ReturnRequest>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.ReturnRequests
                                 .Include(rr => rr.Employee)
                                 .Include(rr => rr.Laptop)
                                 .Where(rr => rr.EmployeeId == employeeId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<ReturnRequest>> GetAllAsync()
        {
            return await _context.ReturnRequests
                                 .Include(rr => rr.Employee)
                                 .Include(rr => rr.Laptop)
                                 .ToListAsync();
        }

        public async Task AddAsync(ReturnRequest returnRequest)
        {
            await _context.ReturnRequests.AddAsync(returnRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReturnRequest returnRequest)
        {
            _context.ReturnRequests.Update(returnRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var returnRequest = await _context.ReturnRequests.FindAsync(id);
            if (returnRequest != null)
            {
                _context.ReturnRequests.Remove(returnRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}