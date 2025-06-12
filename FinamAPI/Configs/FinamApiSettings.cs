namespace FinamAPI.Configs
{
    public class FinamApiSettings
    {
        public const string SectionName = "FinamApiSettings";

        public string BaseUrl { get; set; }
        public string PublicApiKey { get; set; }
        public string UserAgent { get; set; }
        public string ClientId { get; set; }
        public RateLimitSettings RateLimits { get; set; }
        public HttpClientSettings HttpClientSettings { get; set; }
    }

    public class RateLimitSettings
    {
        public int Securities { get; set; }
        public int Portfolio { get; set; }
        public int Orders { get; set; }
        public int Stops { get; set; }
        public int Candles { get; set; }
        public int TimeWindowMinutes { get; set; }
    }

    public class HttpClientSettings
    {
        public int TimeoutSeconds { get; set; }
        public int MaxRetryAttempts { get; set; }
        public int RetryDelayMilliseconds { get; set; }
    }
}
