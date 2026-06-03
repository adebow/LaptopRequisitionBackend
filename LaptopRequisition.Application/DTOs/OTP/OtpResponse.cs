using System.Text.Json.Serialization; // For JsonPropertyName

namespace LaptopRequisition.Application.DTOs.OTP
{
    public class OtpResponse
    {
        [JsonPropertyName("isSuccessful")]
        public bool IsSuccessful { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonPropertyName("data")]
        public OtpData? Data { get; set; }
    }

    public class OtpData
    {
        [JsonPropertyName("otp")]
        public string? Otp { get; set; }
        [JsonPropertyName("retrievalCode")]
        public string? RetrievalCode { get; set; }
        [JsonPropertyName("userReference")] // Added this line
        public string? UserReference { get; set; } // Added this line
    }
}