using FinamAPI.Configs;
using FinamAPI.Models;
using Microsoft.Extensions.Options;

namespace FinamAPI.Services.HttpClient
{
    public class ApiRateLimiter
    {
        private readonly IReadOnlyDictionary<string, RateLimitInfo> _rateLimits;
        private readonly ILogger<ApiRateLimiter> _logger;
        private readonly object _syncLock = new object();

        public ApiRateLimiter(
            IOptions<FinamApiSettings> settings,
            ILogger<ApiRateLimiter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var settingsValue = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            var rateLimitSettings = settingsValue.RateLimits;

            _rateLimits = new Dictionary<string, RateLimitInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["securities"] = CreateRateLimitInfo(rateLimitSettings.Securities, rateLimitSettings.TimeWindowMinutes),
                ["portfolio"] = CreateRateLimitInfo(rateLimitSettings.Portfolio, rateLimitSettings.TimeWindowMinutes),
                ["orders"] = CreateRateLimitInfo(rateLimitSettings.Orders, rateLimitSettings.TimeWindowMinutes),
                ["stops"] = CreateRateLimitInfo(rateLimitSettings.Stops, rateLimitSettings.TimeWindowMinutes),
                ["candles"] = CreateRateLimitInfo(rateLimitSettings.Candles, rateLimitSettings.TimeWindowMinutes)
            };
        }

        public async Task WaitIfNeeded(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("Эндпоинт нулевой", nameof(endpoint));

            if (!_rateLimits.TryGetValue(endpoint, out var limitInfo))
                return;

            TimeSpan delay;

            lock (_syncLock)
            {
                if (limitInfo.CanMakeRequest())
                {
                    limitInfo.DecrementRequests();
                    return;
                }

                delay = limitInfo.GetTimeUntilReset();
            }

            _logger.LogWarning("Rate limit exceeded for {Endpoint}. Waiting {DelaySeconds} seconds...",
                endpoint, delay.TotalSeconds);

            await Task.Delay(delay).ConfigureAwait(false);

            lock (_syncLock)
            {
                limitInfo.DecrementRequests();
            }
        }

        private static RateLimitInfo CreateRateLimitInfo(int requestLimit, int timeWindowMinutes)
        {
            return new RateLimitInfo(requestLimit, TimeSpan.FromMinutes(timeWindowMinutes));
        }
    }
}
