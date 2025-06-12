using System.Text.Json.Serialization;

namespace FinamAPI.Models
{
    public class Security
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("board")]
        public string Board { get; set; }

        [JsonPropertyName("market")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Market Market { get; set; }

        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        [JsonPropertyName("lotSize")]
        public int LotSize { get; set; }

        [JsonPropertyName("minStep")]
        public decimal MinStep { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; }

        [JsonPropertyName("properties")]
        public int PropertiesValue { get; set; }

        [JsonIgnore]
        public SecurityProperties Properties => new SecurityProperties(PropertiesValue);

        [JsonPropertyName("timeZoneName")]
        public string TimeZoneName { get; set; }

        [JsonPropertyName("bpCost")]
        public decimal BpCost { get; set; }

        [JsonPropertyName("accruedInterest")]
        public decimal AccruedInterest { get; set; }

        [JsonPropertyName("priceSign")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriceSign PriceSign { get; set; }

        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("lotDivider")]
        public int LotDivider { get; set; }
    }

    public class Candle
    {
        [JsonPropertyName("open")]
        public DecimalValue Open { get; set; }

        [JsonPropertyName("close")]
        public DecimalValue Close { get; set; }

        [JsonPropertyName("high")]
        public DecimalValue High { get; set; }

        [JsonPropertyName("low")]
        public DecimalValue Low { get; set; }

        [JsonPropertyName("volume")]
        public int Volume { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }

    public class SecurityProperties
    {
        private readonly int _propertiesValue;

        public SecurityProperties(int propertiesValue)
        {
            _propertiesValue = propertiesValue;
        }

        public bool IsTradedOnExchange => HasProperty(SecurityProperty.TradedOnExchange);
        public bool IsBrokerTradingAllowed => HasProperty(SecurityProperty.BrokerTradingAllowed);
        public bool AreMarketOrdersAllowed => HasProperty(SecurityProperty.MarketOrdersAllowed);
        public bool IsMarginal => HasProperty(SecurityProperty.IsMarginal);
        public bool IsCallOption => HasProperty(SecurityProperty.IsCallOption);
        public bool IsPutOption => HasProperty(SecurityProperty.IsPutOption);
        public bool IsAllowedForResidents => HasProperty(SecurityProperty.AllowedForResidents);
        public bool IsAllowedForNonResidents => HasProperty(SecurityProperty.AllowedForNonResidents);

        private bool HasProperty(SecurityProperty property)
        {
            return ((SecurityProperty)_propertiesValue & property) == property;
        }

        public static SecurityProperty ParseProperties(int propertiesValue)
        {
            return (SecurityProperty)propertiesValue;
        }
    }

    public class SecurityFilter
    {
        public string Board { get; set; }
        public string Seccode { get; set; }
    }
}
