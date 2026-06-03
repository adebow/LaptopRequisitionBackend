using Refit;
using LaptopRequisition.Application.DTOs.Notification;
using LaptopRequisition.Application.DTOs.OTP; // For OtpBase
using System.Threading.Tasks;

namespace LaptopRequisition.Application.Interfaces.External
{
    public interface INotificationApi
    {
        [Post("/api/notifications/send")]
        Task<ApiResponse<OtpBase>> SendNotificationAsync([Body] NotificationRequest request);
    }
}