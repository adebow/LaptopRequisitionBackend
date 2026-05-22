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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> GetByStaffIdAsync(string staffId)
        {
            return await _context.Employees
                                 .FirstOrDefaultAsync(e => e.StaffId == staffId);
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees
                                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                                 .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}