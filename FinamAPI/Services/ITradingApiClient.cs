using FinamAPI.Models;

namespace FinamAPI.Services
{
    public interface ITradingApiClient
    {
        Task<long> CreateOrderAsync(NewOrderRequest request);
        Task<IEnumerable<Order>> GetOrdersAsync(string clientId, bool includeMatched = false, bool includeCanceled = false, bool includeActive = true);
        Task CancelOrderAsync(CancelOrderRequest request);
        Task<IEnumerable<Security>> GetSecuritiesAsync(SecurityFilter filter = null);
        Task<Portfolio> GetPortfolioAsync(PortfolioFilter filter);
    }
}
