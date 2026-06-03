using LaptopRequisition.Application.DTOs.Admin;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync();
    }
}