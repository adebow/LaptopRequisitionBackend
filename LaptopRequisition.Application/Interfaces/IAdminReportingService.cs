using LaptopRequisition.Application.DTOs.Admin.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;
using System; // Added for DateTime

namespace LaptopRequisition.Application.Interfaces
{
    public interface IAdminReportingService
    {
        Task<LaptopUtilizationReportDto> GetLaptopUtilizationReportAsync();
        Task<IEnumerable<RequestTrendReportDto>> GetRequestTrendReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EmployeeActivityReportDto>> GetEmployeeActivityReportAsync(DateTime startDate, DateTime endDate);
    }
}