using FinamAPI.Models;
using FinamAPI.Services.HttpClient;
using FinamAPI.Services.Interfaces;
using System.Text.Json;

namespace FinamAPI.Services.Implementations
{
    public class StopService : IStopService
    {
        private readonly HttpClientService _httpClient;
        private readonly ApiRateLimiter _rateLimiter;

        public StopService(HttpClientService httpClient, ApiRateLimiter rateLimiter)
        {
            _httpClient = httpClient;
            _rateLimiter = rateLimiter;
        }

        public async Task<ApiResponse<List<Stop>>> GetStopsAsync(
            string clientId, bool includeExecuted = false,
            bool includeCanceled = false, bool includeActive = true)
        {
            await _rateLimiter.WaitIfNeeded("stops");

            var endpoint = $"/api/v1/stops?clientId={clientId}" +
                          $"&includeExecuted={includeExecuted}" +
                          $"&includeCanceled={includeCanceled}" +
                          $"&includeActive={includeActive}";

            return await _httpClient.GetAsync<ApiResponse<List<Stop>>>(endpoint);
        }

        public async Task<ApiResponse<StopResponse>> PlaceStopOrderAsync(StopRequest request)
        {
            await _rateLimiter.WaitIfNeeded("stops");
            return await _httpClient.PostAsync<StopRequest, ApiResponse<StopResponse>>(
                "/api/v1/stops", request);
        }

        public async Task<ApiResponse<bool>> CancelStopOrderAsync(CancelStopRequest request)
        {
            await _rateLimiter.WaitIfNeeded("stops");

            var response = await _httpClient.DeleteAsync(
                $"/api/v1/stops?clientId={request.ClientId}&stopId={request.StopId}");

            var content = await response.Content.ReadAsStringAsync();
            if (content != null)
                return JsonSerializer.Deserialize<ApiResponse<bool>>(content);
            else
                return null;
        }
    }
}
