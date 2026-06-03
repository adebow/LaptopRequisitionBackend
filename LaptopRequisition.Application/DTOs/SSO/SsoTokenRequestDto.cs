using Refit; // For [AliasAs]

namespace LaptopRequisition.Application.DTOs.SSO
{
    public class SsoTokenRequestDto
    {
        [AliasAs("grant_type")]
        public string GrantType { get; set; } = "password"; // Fixed value
        [AliasAs("client_id")]
        public required string ClientId { get; set; } // Added required
        [AliasAs("client_secret")]
        public required string ClientSecret { get; set; } // Added required
        [AliasAs("username")]
        public required string Username { get; set; } // Added required
        [AliasAs("password")]
        public required string Password { get; set; } // Added required
    }
}