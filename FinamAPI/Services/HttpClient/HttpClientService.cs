using FinamAPI.Configs;
using FinamAPI.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace FinamAPI.Services.HttpClient
{
    public class HttpClientService
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly FinamApiSettings _settings;

        public HttpClientService(
             System.Net.Http.HttpClient httpClient,
            IOptions<FinamApiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _settings.UserAgent);
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _settings.PublicApiKey);
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<T>(response);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var content = JsonSerializer.Serialize(request,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var response = await _httpClient.PostAsync(
                endpoint,
                new StringContent(content, Encoding.UTF8, "application/json"));

            return await HandleResponse<TResponse>(response);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _httpClient.DeleteAsync(endpoint);
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<ApiResponse<object>>(content);
                throw new ApiException(
                    error?.Error?.Message ?? "API request failed",
                    response.StatusCode);
            }

            return JsonSerializer.Deserialize<T>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

       
    }
}
