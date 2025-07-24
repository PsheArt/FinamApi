using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FinamAPI.Configs;
using FinamAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinamAPI.Services.HttpClient
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
    }

    public class HttpClientService : IHttpClientService, IDisposable
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly FinamApiSettings _settings;
        private readonly ILogger<HttpClientService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public HttpClientService(
            System.Net.Http.HttpClient httpClient,
            IOptions<FinamApiSettings> settings,
            ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _settings.UserAgent);
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _settings.PublicApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));

            using var response = await _httpClient.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
            return await HandleResponse<T>(response, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var content = new StringContent(
                JsonSerializer.Serialize(request, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.PostAsync(endpoint, content, cancellationToken).ConfigureAwait(false);
            return await HandleResponse<TResponse>(response, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));

            using var response = await _httpClient.DeleteAsync(endpoint, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError("Delete operation failed. Endpoint: {Endpoint}, Status: {StatusCode}, Error: {Error}",
                    endpoint, response.StatusCode, errorContent);
            }

            return response.IsSuccessStatusCode;
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("API request failed. Status: {StatusCode}, Response: {Response}",
                    response.StatusCode, content);

                try
                {
                    var error = JsonSerializer.Deserialize<ApiResponse<object>>(content, _jsonOptions);
                    throw new ApiException(
                        error?.Error?.Message ?? "API request failed",
                        response.StatusCode);
                }
                catch (JsonException jsonEx)
                {
                    throw new ApiException($"Failed to parse error response: {content}", response.StatusCode, jsonEx);
                }
            }

            try
            {
                return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize response. Content: {Content}", content);
                throw;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}