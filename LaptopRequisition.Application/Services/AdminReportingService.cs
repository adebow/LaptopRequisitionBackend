using LaptopRequisition.Application.DTOs.Admin.Reports;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Services
{
    public class AdminReportingService : IAdminReportingService
    {
        private readonly ILaptopRepository _laptopRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AdminReportingService(ILaptopRepository laptopRepository,
                                     IRequestRepository requestRepository,
                                     IReturnRequestRepository returnRequestRepository,
                                     IEmployeeRepository employeeRepository)
        {
            _laptopRepository = laptopRepository;
            _requestRepository = requestRepository;
            _returnRequestRepository = returnRequestRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<LaptopUtilizationReportDto> GetLaptopUtilizationReportAsync()
        {
            var totalLaptops = await _laptopRepository.CountAllAsync();
            var availableLaptops = await _laptopRepository.CountAvailableAsync();
            // Corrected: Use LaptopStatus.UnderRepair instead of LaptopStatus.InRepair
            var inRepairLaptops = await _laptopRepository.CountByStatusAsync(LaptopStatus.UnderRepair); 

            var assignedLaptops = totalLaptops - availableLaptops - inRepairLaptops; // Calculate assigned

            return new LaptopUtilizationReportDto
            {
                TotalLaptops = totalLaptops,
                AssignedLaptops = assignedLaptops,
                AvailableLaptops = availableLaptops,
                InRepairLaptops = inRepairLaptops
            };
        }

        public async Task<IEnumerable<RequestTrendReportDto>> GetRequestTrendReportAsync(DateTime startDate, DateTime endDate)
        {
            var requests = await _requestRepository.GetAllAsync(); // Get all requests
            var returnRequests = await _returnRequestRepository.GetAllAsync(); // Get all return requests

            var reportData = new List<RequestTrendReportDto>();

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var newRequests = requests.Count(r => r.CreatedAt.Date == date && r.Status == RequestStatus.Pending);
                var approvedRequests = requests.Count(r => r.ApprovedRejectedAt?.Date == date && r.Status == RequestStatus.Approved);
                var rejectedRequests = requests.Count(r => r.ApprovedRejectedAt?.Date == date && r.Status == RequestStatus.Rejected);
                var completedRequests = requests.Count(r => r.ReceiptConfirmedAt?.Date == date && r.Status == RequestStatus.Completed);

                reportData.Add(new RequestTrendReportDto
                {
                    Date = date,
                    NewRequests = newRequests,
                    ApprovedRequests = approvedRequests,
                    RejectedRequests = rejectedRequests,
                    CompletedRequests = completedRequests
                });
            }

            return reportData;
        }

        public async Task<IEnumerable<EmployeeActivityReportDto>> GetEmployeeActivityReportAsync(DateTime startDate, DateTime endDate)
        {
            var employees = await _employeeRepository.GetAllWithDepartmentAndRoleAsync(); // Get all employees with their details
            var requests = await _requestRepository.GetAllAsync();
            var returnRequests = await _returnRequestRepository.GetAllAsync();
            var laptops = await _laptopRepository.GetAllAsync();

            var reportData = new List<EmployeeActivityReportDto>();

            foreach (var employee in employees)
            {
                var employeeRequests = requests.Where(r => r.EmployeeId == employee.Id && r.CreatedAt >= startDate && r.CreatedAt <= endDate).ToList();
                var employeeReturnRequests = returnRequests.Where(rr => rr.EmployeeId == employee.Id && rr.CreatedAt >= startDate && rr.CreatedAt <= endDate).ToList();
                var assignedLaptopsCount = laptops.Count(l => l.AssignedToEmployeeId == employee.Id);

                reportData.Add(new EmployeeActivityReportDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.FullName,
                    StaffId = employee.StaffId,
                    TotalRequests = employeeRequests.Count,
                    ApprovedRequests = employeeRequests.Count(r => r.Status == RequestStatus.Approved),
                    RejectedRequests = employeeRequests.Count(r => r.Status == RequestStatus.Rejected),
                    TotalReturnRequests = employeeReturnRequests.Count,
                    ApprovedReturnRequests = employeeReturnRequests.Count(rr => rr.Status == ReturnRequestStatus.Approved.ToString()),
                    RejectedReturnRequests = employeeReturnRequests.Count(rr => rr.Status == ReturnRequestStatus.Rejected.ToString()),
                    AssignedLaptopsCount = assignedLaptopsCount
                });
            }

            return reportData.OrderBy(e => e.EmployeeName);
        }
    }
}