using FinamAPI.Models;
using FinamAPI.Services.HttpClient;
using FinamAPI.Services.Interfaces;

namespace FinamAPI.Services.Implementations
{
    public class MarketDataService : IMarketDataService
    {
        private readonly HttpClientService _httpClient;
        private readonly ApiRateLimiter _rateLimiter;

        public MarketDataService(HttpClientService httpClient, ApiRateLimiter rateLimiter)
        {
            _httpClient = httpClient;
            _rateLimiter = rateLimiter;
        }

        public async Task<ApiResponse<List<Security>>> GetSecuritiesAsync()
        {
            await _rateLimiter.WaitIfNeeded("securities");
            return await _httpClient.GetAsync<ApiResponse<List<Security>>>("/api/v1/securities");
        }

        public async Task<ApiResponse<List<Candle>>> GetCandlesAsync(
            string securityCode, DateTime from, DateTime to, string timeframe)
        {
            await _rateLimiter.WaitIfNeeded("candles");

            var endpoint = $"/api/v1/candles?securityCode={securityCode}" +
                          $"&from={from:yyyy-MM-ddTHH:mm:ss}" +
                          $"&to={to:yyyy-MM-ddTHH:mm:ss}" +
                          $"&timeframe={timeframe}";

            return await _httpClient.GetAsync<ApiResponse<List<Candle>>>(endpoint);
        }
    }
  
}
