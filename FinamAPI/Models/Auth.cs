using System.Text.Json.Serialization;

namespace FinamAPI.Models
{
    public class AuthRequest
    {

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }
    }
    public class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonIgnore]
        public DateTime TokenExpiration { get; set; }

        [JsonIgnore]
        public bool IsSuccess => !string.IsNullOrEmpty(AccessToken);
    }

    public class AuthErrorResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }
}
