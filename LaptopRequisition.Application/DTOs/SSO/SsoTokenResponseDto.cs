using System.Text.Json.Serialization; // For [JsonPropertyName]

namespace LaptopRequisition.Application.DTOs.SSO
{
    public class SsoTokenResponseDto
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}