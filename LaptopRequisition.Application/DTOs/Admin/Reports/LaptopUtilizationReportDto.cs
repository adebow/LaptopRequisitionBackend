namespace LaptopRequisition.Application.DTOs.Admin.Reports
{
    public class LaptopUtilizationReportDto
    {
        public int TotalLaptops { get; set; }
        public int AssignedLaptops { get; set; }
        public int AvailableLaptops { get; set; }
        public int InRepairLaptops { get; set; }
        public double UtilizationRate => TotalLaptops > 0 ? (double)AssignedLaptops / TotalLaptops : 0;
    }
}