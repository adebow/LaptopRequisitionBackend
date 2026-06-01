using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using LaptopRequisition.Application.Configurations; // Added for RecycleBinSettings

namespace LaptopRequisition.Application.Services
{
    public class RecycleBinService : IRecycleBinService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly RecycleBinSettings _recycleBinSettings;

        public RecycleBinService(IEmployeeRepository employeeRepository, IOptions<RecycleBinSettings> recycleBinSettingsOptions)
        {
            _employeeRepository = employeeRepository;
            _recycleBinSettings = recycleBinSettingsOptions.Value;
        }

        public async Task CleanUpRecycleBinAsync()
        {
            // Retrieve all soft-deleted employees
            var deletedEmployees = await _employeeRepository.GetDeletedEmployeesAsync();

            // Filter employees whose UpdatedAt (which was set on soft-delete) is older than the permanent deletion threshold
            var employeesToPurge = deletedEmployees
                .Where(e => e.UpdatedAt.AddDays(_recycleBinSettings.PermanentDeletionDays) <= DateTime.UtcNow)
                .ToList();

            foreach (var employee in employeesToPurge)
            {
                await _employeeRepository.HardDeleteAsync(employee.Id);
                // Optionally, log the permanent deletion
                Console.WriteLine($"Employee {employee.FullName} (ID: {employee.Id}) permanently deleted from recycle bin.");
            }
        }
    }
}