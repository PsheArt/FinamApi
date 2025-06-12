using System.Text.Json.Serialization;

namespace FinamAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Market
    {
        [JsonPropertyName("Stock")]
        Stock,

        [JsonPropertyName("Forts")]
        Forts,

        [JsonPropertyName("Spbex")]
        Spbex,

        [JsonPropertyName("Mma")]
        Mma,

        [JsonPropertyName("Ets")]
        Ets,

        [JsonPropertyName("Bonds")]
        Bonds,

        [JsonPropertyName("Options")]
        Options
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BuySell
    {
        [JsonPropertyName("Buy")]
        Buy,

        [JsonPropertyName("Sell")]
        Sell
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderValidBeforeType
    {
        [JsonPropertyName("TillEndSession")]
        TillEndSession,

        [JsonPropertyName("TillCancelled")]
        TillCancelled,

        [JsonPropertyName("ExactTime")]
        ExactTime
    }
}
