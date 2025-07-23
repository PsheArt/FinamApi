using System.Net;
using System.Text.Json.Serialization;

namespace FinamAPI.Models
{

    public class RateLimitInfo
    {
        public int MaxRequests { get; }
        public TimeSpan TimeWindow { get; }
        public int RemainingRequests { get; private set; }
        public DateTime WindowStartTime { get; private set; }

        public RateLimitInfo(int maxRequests, TimeSpan timeWindow)
        {
            MaxRequests = maxRequests;
            TimeWindow = timeWindow;
            RemainingRequests = maxRequests;
            WindowStartTime = DateTime.UtcNow;
        }

        public bool CanMakeRequest()
        {
            ResetWindowIfExpired();
            return RemainingRequests > 0;
        }

        public void DecrementRequests()
        {
            ResetWindowIfExpired();
            if (RemainingRequests > 0)
                RemainingRequests--;
        }

        public TimeSpan GetTimeUntilReset()
        {
            var timePassed = DateTime.UtcNow - WindowStartTime;
            return timePassed < TimeWindow
                ? TimeWindow - timePassed
                : TimeSpan.Zero;
        }

        private void ResetWindowIfExpired()
        {
            if (DateTime.UtcNow - WindowStartTime >= TimeWindow)
            {
                RemainingRequests = MaxRequests;
                WindowStartTime = DateTime.UtcNow;
            }
        }
    }

    public class ApiResponse<T>
    {
        [JsonPropertyName("error")]
        public ErrorInfo? Error { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        public bool IsSuccess => Error == null;
    }

    public class ErrorInfo
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public string? Data { get; set; }
    }

    public class DecimalValue
    {
        [JsonPropertyName("num")]
        public long Num { get; set; }

        [JsonPropertyName("scale")]
        public int Scale { get; set; }

        public decimal ToDecimal() => Num * (decimal)Math.Pow(10, -Scale);

        public static DecimalValue FromDecimal(decimal value)
        {
            var scale = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
            return new DecimalValue
            {
                Num = (long)(value * (decimal)Math.Pow(10, scale)),
                Scale = scale
            };
        }
    }

    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(string message, HttpStatusCode statusCode, System.Text.Json.JsonException jsonEx)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiException(string? message, HttpStatusCode statusCode) : base(message)
        {
        }
    }

    public class RateLimitExceededException : ApiException
    {
        public TimeSpan RetryAfter { get; }

        public RateLimitExceededException(TimeSpan retryAfter)
            : base("Rate limit exceeded", HttpStatusCode.TooManyRequests)
        {
            RetryAfter = retryAfter;
        }
    }
}
