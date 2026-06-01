using LaptopRequisition.Application.DTOs.Notification; // Added for NotificationRequest
using LaptopRequisition.Application.DTOs.OTP;
using LaptopRequisition.Application.Helpers;
using LaptopRequisition.Application.Interfaces;
using LaptopRequisition.Application.Interfaces.External; // For IOtpApi
using LaptopRequisition.Domain.Enums;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options; // Added for IOptions
using LaptopRequisition.Application.Configurations; // Added for NotificationApiSettings
using System.IO; // Added for Path.Combine, File.ReadAllTextAsync
using System; // Added for AppDomain


namespace LaptopRequisition.Application.Services
{
    public class OtpHelperService : IOtpHelperService
    {
        private readonly IOtpApi _otpService;
        private readonly INotificationApi _notificationApi; 
        private readonly NotificationApiSettings _notificationApiSettings;

        public OtpHelperService(
            IOtpApi otpService,
            INotificationApi notificationApi,
            IOptions<NotificationApiSettings> notificationApiSettingsOptions) 
        {
            _otpService = otpService;
            _notificationApi = notificationApi; 
            _notificationApiSettings = notificationApiSettingsOptions.Value;
        }

        public async Task<ResponseModel<ResponseCodeEnum, OtpResponse>> GenerateOtpAsync(string userRef)
        {
            var otpResult = await _otpService.GenerateOtpAsync(new GenerateOtpRequest
            {
                UserReference = userRef,
                Time = 5, 
                OtpDigit = "SixDigits" 
            });

            if (!otpResult.IsSuccessStatusCode || otpResult.Content is null || !otpResult.Content.IsSuccessful)
            {
                var errorContent = otpResult.Error?.Content;
                var errorMessage = "Error occurred while generating OTP";
                if (!string.IsNullOrEmpty(errorContent))
                {
                    try
                    {
                        var otpBaseError = JsonConvert.DeserializeObject<OtpBase>(errorContent);
                        errorMessage = otpBaseError?.Message ?? errorMessage;
                    }
                    catch (JsonException) { }
                }
                return ResponseModel<ResponseCodeEnum, OtpResponse>.Failure(ResponseCodeEnum.ErrorOccuredTryAgain, errorMessage);
            }
            
            var otpValue = otpResult.Content.Data!.Otp!;
            var purpose = "Secure Authentication"; 

            var sendResult = await SendOtpEmailAsync(userRef, otpValue, purpose);

            if (!sendResult.IsSuccessful)
            {
                return ResponseModel<ResponseCodeEnum, OtpResponse>.Failure(sendResult.Code, sendResult.Message);
            }
           
            
            return ResponseModel<ResponseCodeEnum, OtpResponse>.Success(otpResult.Content, ResponseCodeEnum.OperationSuccessful);
        }

        public async Task<ResponseModel<ResponseCodeEnum, OtpResponse>> ValidateOtpAsync(string retrievalCode, string otp)
        {
            var validationResult = await _otpService.ValidateOtpAsync(new ValidateOtpRequest
            {
                RetrievalCode = retrievalCode,
                Otp = otp
            });

            if (!validationResult.IsSuccessStatusCode || validationResult.Content is null || !validationResult.Content.IsSuccessful)
            {
                var errorContent = validationResult.Error?.Content;
                var errorMessage = "OTP validation failed.";
                if (!string.IsNullOrEmpty(errorContent))
                {
                    try
                    {
                        var otpBaseError = JsonConvert.DeserializeObject<OtpBase>(errorContent);
                        errorMessage = otpBaseError?.Message ?? errorMessage;
                    }
                    catch (JsonException) { }
                }
                return ResponseModel<ResponseCodeEnum, OtpResponse>.Failure(ResponseCodeEnum.OtpValidationFailed, errorMessage);
            }

            return ResponseModel<ResponseCodeEnum, OtpResponse>.Success(validationResult.Content, ResponseCodeEnum.OperationSuccessful);
        }

        public async Task<ResponseModel<ResponseCodeEnum, OtpBase>> CheckOtpValidityAsync(string retrievalCode, string userRef)
        {
            var validityResult = await _otpService.CheckOtpValidityAsync(retrievalCode, userRef);

            if (!validityResult.IsSuccessStatusCode || validityResult.Content is null || !validityResult.Content.IsSuccessful)
            {
                var errorContent = validityResult.Error?.Content;
                var errorMessage = "OTP validity check failed.";
                if (!string.IsNullOrEmpty(errorContent))
                {
                    try
                    {
                        var otpBaseError = JsonConvert.DeserializeObject<OtpBase>(errorContent);
                        errorMessage = otpBaseError?.Message ?? errorMessage;
                    }
                    catch (JsonException) { }
                }
                return ResponseModel<ResponseCodeEnum, OtpBase>.Failure(ResponseCodeEnum.OtpValidationFailed, errorMessage);
            }

            return ResponseModel<ResponseCodeEnum, OtpBase>.Success(validityResult.Content, ResponseCodeEnum.OperationSuccessful);
        }
        

        private async Task<ResponseModel<ResponseCodeEnum, OtpResponse>> SendOtpEmailAsync(
            string email,
            string otp,
            string purpose)
        {
            Console.WriteLine($"Sending OTP notification to {email} via Email"); 

            var notificationRequest = new NotificationRequest
            {
                Channels = new List<string> { "Email" },
                From = _notificationApiSettings.FromEmail,
                To = email,
                Subject = "Secure Authentication",
                Message = await BuildOtpEmailBodyAsync(otp, purpose)
            };

            Console.WriteLine($"[Notification Request] Sending to: {notificationRequest.To}, From: {notificationRequest.From}, Subject: {notificationRequest.Subject}, Channels: {string.Join(", ", notificationRequest.Channels)}");
            Console.WriteLine($"[Notification Request] Message (first 100 chars): {notificationRequest.Message?.Substring(0, Math.Min(notificationRequest.Message.Length, 100))}");


            var otpResp = await _notificationApi.SendNotificationAsync(notificationRequest);

            Console.WriteLine($"[Notification API Response] IsSuccessStatusCode: {otpResp.IsSuccessStatusCode}, Content: {otpResp.Content}, Error: {otpResp.Error?.Content}");


            if (!otpResp.IsSuccessStatusCode || otpResp.Content is null || !otpResp.Content.IsSuccessful)
            {
                Console.Error.WriteLine($"[Otp-Email-Failed] Failed to send OTP email to {email}. Error: {otpResp.Error?.Content}");
                var error = JsonConvert.DeserializeObject<OtpBase>(
                    otpResp.Error?.Content ?? string.Empty);

                return ResponseModel<ResponseCodeEnum, OtpResponse>.Failure(ResponseCodeEnum.ErrorOccuredTryAgain,
                    error?.Message ?? "Failed to send OTP email.");
            }

            Console.WriteLine($"[Otp-Email-Sent] OTP email successfully sent to {email}");
            return ResponseModel<ResponseCodeEnum, OtpResponse>.Success(null!, ResponseCodeEnum.OperationSuccessful);
        }

        private async Task<string> BuildOtpEmailBodyAsync(string otp, string purpose)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "otp.html");
            
            if (!File.Exists(templatePath))
            {
                return $"Dear User,\n\nYour One-Time Password (OTP) for {purpose} is: {otp}\n\nThis code is valid for 5 minutes. Please do not share it with anyone.\n\nBest regards,\nThe LRS Team";
            }

            var body = await File.ReadAllTextAsync(templatePath);
            
            return body
                .Replace("{{otp}}", otp)
                .Replace("{{purpose}}", purpose)
                .Replace("{{currentYear}}", DateTime.UtcNow.Year.ToString()); // Added current year replacement
        }
    }
}