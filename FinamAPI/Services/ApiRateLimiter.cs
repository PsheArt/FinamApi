using FinamAPI.Configs;
using FinamAPI.Models;
using Microsoft.Extensions.Options;

namespace FinamAPI.Services
{
    public class ApiRateLimiter
    {
        private readonly Dictionary<string, RateLimitInfo> _rateLimits;
        private readonly ILogger<ApiRateLimiter> _logger;

        public ApiRateLimiter(
            IOptions<FinamApiSettings> settings,
            ILogger<ApiRateLimiter> logger)
        {
            _logger = logger;
            var rateLimitSettings = settings.Value.RateLimits;

            _rateLimits = new Dictionary<string, RateLimitInfo>
            {
                { "securities", new RateLimitInfo(rateLimitSettings.Securities, TimeSpan.FromMinutes(rateLimitSettings.TimeWindowMinutes)) },
                { "portfolio", new RateLimitInfo(rateLimitSettings.Portfolio, TimeSpan.FromMinutes(rateLimitSettings.TimeWindowMinutes)) },
                { "orders", new RateLimitInfo(rateLimitSettings.Orders, TimeSpan.FromMinutes(rateLimitSettings.TimeWindowMinutes)) },
                { "stops", new RateLimitInfo(rateLimitSettings.Stops, TimeSpan.FromMinutes(rateLimitSettings.TimeWindowMinutes)) },
                { "candles", new RateLimitInfo(rateLimitSettings.Candles, TimeSpan.FromMinutes(rateLimitSettings.TimeWindowMinutes)) }
            };
        }

        public async Task WaitIfNeeded(string endpoint)
        {
            if (!_rateLimits.TryGetValue(endpoint, out var limitInfo))
                return;

            if (limitInfo.CanMakeRequest())
            {
                limitInfo.DecrementRequests();
                return;
            }

            var delay = limitInfo.GetTimeUntilReset();
            _logger.LogWarning($"Rate limit exceeded for {endpoint}. Waiting {delay.TotalSeconds} seconds...");
            await Task.Delay(delay);

            limitInfo.DecrementRequests();
        }
    }
}
