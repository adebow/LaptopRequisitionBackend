using Refit;
using LaptopRequisition.Application.DTOs.OTP;
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces.External
{
    public interface IOtpApi
    {
        [Post("/api/generate-otp")]
        Task<ApiResponse<OtpResponse>> GenerateOtpAsync([Body] GenerateOtpRequest request);

        [Get("/api/check-otp-validity/{retrievalCode}/{userRef}")]
        Task<ApiResponse<OtpBase>> CheckOtpValidityAsync(string retrievalCode, string userRef);

        [Post("/api/validate-otp")]
        Task<ApiResponse<OtpResponse>> ValidateOtpAsync([Body] ValidateOtpRequest request);
    }
}