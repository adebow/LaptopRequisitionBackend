using LaptopRequisition.Application.DTOs.OTP;
using System.Threading.Tasks;
using LaptopRequisition.Domain.Enums;
using LaptopRequisition.Application.Helpers;

namespace LaptopRequisition.Application.Interfaces
{
    public interface IOtpHelperService
    {
        Task<ResponseModel<ResponseCodeEnum, OtpResponse>> GenerateOtpAsync(string userRef);
        Task<ResponseModel<ResponseCodeEnum, OtpResponse>> ValidateOtpAsync(string retrievalCode, string otp);
        Task<ResponseModel<ResponseCodeEnum, OtpBase>> CheckOtpValidityAsync(string retrievalCode, string userRef); // Added back
    }
}