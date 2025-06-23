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

    public class TradingService : ITradingService
    {
        private readonly HttpClientService _httpClient;
        private readonly ApiRateLimiter _rateLimiter;

        public TradingService(HttpClientService httpClient, ApiRateLimiter rateLimiter)
        {
            _httpClient = httpClient;
            _rateLimiter = rateLimiter;
        }

        public async Task<ApiResponse<OrderResponse>> PlaceOrderAsync(OrderRequest request)
        {
            await _rateLimiter.WaitIfNeeded("orders");
            return await _httpClient.PostAsync<OrderRequest, ApiResponse<OrderResponse>>(
                "/api/v1/orders", request);
        }

        public async Task<ApiResponse<bool>> CancelOrderAsync(long transactionId)
        {
            await _rateLimiter.WaitIfNeeded("orders");
            var response = await _httpClient.DeleteAsync($"/api/v1/orders/{transactionId}");
            return new ApiResponse<bool> { Data = response.IsSuccessStatusCode };
        }

        public async Task<ApiResponse<OrderResponse>> PlaceStopOrderAsync(StopOrderRequest request)
        {
            await _rateLimiter.WaitIfNeeded("stops");
            return await _httpClient.PostAsync<StopOrderRequest, ApiResponse<OrderResponse>>(
                "/api/v1/stops", request);
        }

        public async Task<ApiResponse<bool>> CancelStopOrderAsync(long transactionId)
        {
            await _rateLimiter.WaitIfNeeded("stops");
            var response = await _httpClient.DeleteAsync($"/api/v1/stops/{transactionId}");
            return new ApiResponse<bool> { Data = response.IsSuccessStatusCode };
        }
    }
}
