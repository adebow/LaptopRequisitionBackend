using LaptopRequisition.Application.DTOs.Dashboard;
using System;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetEmployeeDashboardSummaryAsync(Guid employeeId);
    }
}