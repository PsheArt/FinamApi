using FinamAPI.Models;

namespace FinamAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync();
        Task<string> GetAccessTokenAsync();
        bool IsTokenValid();
    }
}

