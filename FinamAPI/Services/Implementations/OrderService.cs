using FinamAPI.Models;
using FinamAPI.Services.HttpClient;
using FinamAPI.Services.Interfaces;
using System.Text.Json;

namespace FinamAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientService _httpClient;
        private readonly ApiRateLimiter _rateLimiter;
        private const string OrdersResource = "orders";

        public OrderService(IHttpClientService httpClient, IApiRateLimiter rateLimiter)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _rateLimiter = rateLimiter ?? throw new ArgumentNullException(nameof(rateLimiter));
        }

        public async Task<ApiResponse<List<Order>>> GetOrdersAsync(
            string clientId,
            bool includeMatched = false,
            bool includeCanceled = false,
            bool includeActive = true)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("Client ID cannot be empty", nameof(clientId));

            await _rateLimiter.WaitIfNeeded(OrdersResource);

            var queryParams = new Dictionary<string, string>
            {
                ["includeMatched"] = includeMatched.ToString(),
                ["includeCanceled"] = includeCanceled.ToString(),
                ["includeActive"] = includeActive.ToString()
            };

            var endpoint = BuildEndpoint($"v1/accounts/{clientId}/orders", queryParams);

            return await _httpClient.GetAsync<ApiResponse<List<Order>>>(endpoint);
        }

        public async Task<ApiResponse<OrderResponse>> PlaceOrderAsync(OrderRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _rateLimiter.WaitIfNeeded(OrdersResource);

            return await _httpClient.PostAsync<OrderRequest, ApiResponse<OrderResponse>>(
                "v1/accounts/orders",
                request);
        }

        public async Task<ApiResponse<bool>> CancelOrderAsync(CancelOrderRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.ClientId))
                throw new ArgumentException("Client ID cannot be empty", nameof(request.ClientId));
            if (string.IsNullOrWhiteSpace(request.TransactionId))
                throw new ArgumentException("Transaction ID cannot be empty", nameof(request.TransactionId));

            await _rateLimiter.WaitIfNeeded(OrdersResource);

            var endpoint = $"v1/accounts/{request.ClientId}/orders/{request.TransactionId}";
            var response = await _httpClient.DeleteAsync(endpoint);

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<bool>>(content);
        }

        private string BuildEndpoint(string basePath, IDictionary<string, string> queryParams)
        {
            var queryString = string.Join("&", queryParams
                .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

            return string.IsNullOrEmpty(queryString)
                ? basePath
                : $"{basePath}?{queryString}";
        }
    }
}
