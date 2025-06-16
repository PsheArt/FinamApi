namespace FinamAPI.Services.HttpClient
{
    public static class HttpClientExtensions
    {
        public static async Task<string> GetResponseContentAsync(this HttpRequestException ex)
        {
            try
            {
                var responseProperty = ex.GetType().GetProperty("Response");
                if (responseProperty == null)
                    return string.Empty;

                var response = responseProperty.GetValue(ex) as HttpResponseMessage;
                if (response?.Content == null)
                    return string.Empty;

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
