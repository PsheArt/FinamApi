using FinamAPI.Configs;
using FinamAPI.Services.Implementations;
using FinamAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace FinamAPI.Services.HttpClient
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFinamApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FinamApiSettings>(configuration.GetSection(FinamApiSettings.SectionName));

            services.AddHttpClient<HttpClientService>()
                .AddPolicyHandler((services, request) =>
                    HttpPolicyExtensions.HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            services.GetRequiredService<IOptions<FinamApiSettings>>().Value.HttpClientSettings.MaxRetryAttempts,
                            retryAttempt => TimeSpan.FromMilliseconds(
                                services.GetRequiredService<IOptions<FinamApiSettings>>().Value.HttpClientSettings.RetryDelayMilliseconds),
                            onRetry: (outcome, delay, retryNumber, context) =>
                            {
                                services.GetService<ILogger<HttpClientService>>()?
                                    .LogWarning($"Retry {retryNumber} after {delay.TotalMilliseconds}ms due to: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                            }));

            services.AddSingleton<ApiRateLimiter>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IStopService, StopService>();

            return services;
        }
        private static void ConfigureHttpClient(IServiceProvider provider, System.Net.Http.HttpClient client)
        {
            var settings = provider.GetRequiredService<IOptions<FinamApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(settings.HttpClientSettings.TimeoutSeconds);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration configuration)
        {
            var settings = configuration.GetSection(FinamApiSettings.SectionName).Get<FinamApiSettings>();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(
                    settings.HttpClientSettings.MaxRetryAttempts,
                    retryAttempt => TimeSpan.FromMilliseconds(settings.HttpClientSettings.RetryDelayMilliseconds));
        }
    }
}
