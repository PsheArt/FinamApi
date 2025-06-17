using FinamAPI.Models;
using System.Text.Json;

namespace FinamAPI.Services
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

            var endpoint = $"/api/v1/orders?clientId={clientId}" +
                          $"&includeMatched={includeMatched}" +
                          $"&includeCanceled={includeCanceled}" +
                          $"&includeActive={includeActive}";

            return await _httpClient.GetAsync<ApiResponse<List<Order>>>(endpoint);
        }

        public async Task<ApiResponse<OrderResponse>> PlaceOrderAsync(OrderRequest request)
        {
            await _rateLimiter.WaitIfNeeded("orders");
            return await _httpClient.PostAsync<OrderRequest, ApiResponse<OrderResponse>>(
                "/api/v1/orders", request);
        }

        public async Task<ApiResponse<bool>> CancelOrderAsync(CancelOrderRequest request)
        {
            await _rateLimiter.WaitIfNeeded("orders");

            var response = await _httpClient.DeleteAsync(
                $"/api/v1/orders?clientId={request.ClientId}&transactionId={request.TransactionId}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<bool>>(content);
        }
    }
}
