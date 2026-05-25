using LaptopRequisition.Application.DTOs.SSO;

namespace LaptopRequisition.Application.DTOs
{
    public class LoginResponseDto
    {
        public SsoTokenResponseDto TokenDetails { get; set; } = new SsoTokenResponseDto();
        public EmployeeDto EmployeeDetails { get; set; } = new EmployeeDto(); 
    }
}