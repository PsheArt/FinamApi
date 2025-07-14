using FinamAPI.Configs;
using FinamAPI.Models;
using FinamAPI.Services.HttpClient;
using FinamAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace FinamAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly HttpClientService _httpClient;
        private readonly FinamApiSettings _settings;
        private readonly ILogger<AuthService> _logger;
        private AuthResponse _authResponse;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public AuthService(
            HttpClientService httpClient,
            IOptions<FinamApiSettings> settings,
            ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }
       

        public async Task<AuthResponse> AuthenticateAsync()
        {
            try
            {
                await _semaphore.WaitAsync();

                if (IsTokenValid() && _authResponse != null)
                {
                    return _authResponse;
                }

                var request = new AuthRequest
                {
                    ClientSecret = _settings.PublicApiKey
                };

                var response = await _httpClient.PostAsync<AuthRequest, ApiResponse<AuthResponse>>("v1/sessions", request);
                
                if (response != null)
                {
                    response.Data.TokenExpiration = DateTime.UtcNow.AddSeconds(_authResponse.ExpiresIn - 60);
                    _logger.LogInformation("Проверка подлинности завершена успешно. Срок действия токена истекает в {0}", _authResponse.TokenExpiration);
                }

                return _authResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Не удалось выполнить аутентификацию с использованием кода состояния {StatusCode}", ex.StatusCode);
                var errorContent = await ex.GetResponseContentAsync();
                var error = JsonSerializer.Deserialize<AuthErrorResponse>(errorContent);
                throw new ApiException(error?.ErrorDescription ?? "Не удалось выполнить аутентификацию", ex.StatusCode ?? HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected authentication error");
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (IsTokenValid() && _authResponse != null)
            {
                return _authResponse.AccessToken;
            }

            var response = await AuthenticateAsync();
            return response.AccessToken;
        }

        public bool IsTokenValid()
        {
            return _authResponse != null &&
                   !string.IsNullOrEmpty(_authResponse.AccessToken) &&
                   DateTime.UtcNow < _authResponse.TokenExpiration;
        }
    }
}
