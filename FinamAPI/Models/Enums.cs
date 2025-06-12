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

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriceSign
    {
        [JsonPropertyName("Unspecified")]
        Unspecified,
        [JsonPropertyName("Positive")]
        Positive,
        [JsonPropertyName("NonNegative")]
        NonNegative,
        [JsonPropertyName("Any")]
        Any
    }

    [Flags]
    public enum SecurityProperty
    {
        None = 0,
        TradedOnExchange = 1 << 0,
        BrokerTradingAllowed = 1 << 1,
        MarketOrdersAllowed = 1 << 2,
        IsMarginal = 1 << 3,
        IsCallOption = 1 << 4,
        IsPutOption = 1 << 5,
        AllowedForResidents = 1 << 6,
        AllowedForNonResidents = 1 << 7
    }
}
