using FinamAPI.Models;
using FinamAPI.Services.HttpClient;
using FinamAPI.Services.Interfaces;
using System.Text.Json;

namespace FinamAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly HttpClientService _httpClient;
        private readonly ApiRateLimiter _rateLimiter;

        public OrderService(HttpClientService httpClient, ApiRateLimiter rateLimiter)
        {
            _httpClient = httpClient;
            _rateLimiter = rateLimiter;
        }

        public async Task<ApiResponse<List<Order>>> GetOrdersAsync(
            string clientId, bool includeMatched = false,
            bool includeCanceled = false, bool includeActive = true)
        {
            await _rateLimiter.WaitIfNeeded("orders");

            var endpoint = $"v1/accounts/?clientId={clientId}/orders" +
                          $"&includeMatched={includeMatched}" +
                          $"&includeCanceled={includeCanceled}" +
                          $"&includeActive={includeActive}";

            return await _httpClient.GetAsync<ApiResponse<List<Order>>>(endpoint);
        }

        public async Task<ApiResponse<OrderResponse>> PlaceOrderAsync(OrderRequest request)
        {
            await _rateLimiter.WaitIfNeeded("orders");
            return await _httpClient.PostAsync<OrderRequest, ApiResponse<OrderResponse>>(
                "v1/accounts/", request);
        }

        public async Task<ApiResponse<bool>> CancelOrderAsync(CancelOrderRequest request)
        {
            await _rateLimiter.WaitIfNeeded("orders");

            var response = await _httpClient.DeleteAsync(
                $"v1/accounts/{request.ClientId}/orders/{request.TransactionId}");

            return JsonSerializer.Deserialize<ApiResponse<bool>>(response);
        }
    }
}
