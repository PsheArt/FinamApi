using System.Text.Json.Serialization;

namespace FinamAPI.Models
{
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
}
