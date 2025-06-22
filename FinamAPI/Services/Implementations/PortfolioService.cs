using FinamAPI.Models;
using FinamAPI.Services.HttpClient;
using FinamAPI.Services.Interfaces;
using System.Text;

namespace FinamAPI.Services.Implementations
{
    public class PortfolioService : IPortfolioService
    {
        private readonly HttpClientService _httpClient;
        private readonly ApiRateLimiter _rateLimiter;

        public PortfolioService(HttpClientService httpClient, ApiRateLimiter rateLimiter)
        {
            _httpClient = httpClient;
            _rateLimiter = rateLimiter;
        }

        public async Task<ApiResponse<Portfolio>> GetPortfolioAsync(PortfolioFilter filter)
        {
            await _rateLimiter.WaitIfNeeded("portfolio");

            var query = new StringBuilder($"?clientId={filter.ClientId}");
            query.Append($"&includeCurrencies={filter.IncludeCurrencies.ToString().ToLower()}");
            query.Append($"&includeMoney={filter.IncludeMoney.ToString().ToLower()}");
            query.Append($"&includePositions={filter.IncludePositions.ToString().ToLower()}");
            query.Append($"&includeMaxBuySell={filter.IncludeMaxBuySell.ToString().ToLower()}");

            return await _httpClient.GetAsync<ApiResponse<Portfolio>>($"/api/v1/portfolio{query}");
        }
    }
}