using System.Text.Json.Serialization;

namespace FinamAPI.Models
{
    public class Portfolio
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("content")]
        public PortfolioContent Content { get; set; }

        [JsonPropertyName("equity")]
        public decimal Equity { get; set; }

        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }

        [JsonPropertyName("positions")]
        public List<PositionRow> Positions { get; set; }

        [JsonPropertyName("currencies")]
        public List<CurrencyRow> Currencies { get; set; }

        [JsonPropertyName("money")]
        public List<MoneyRow> Money { get; set; }
    }

    public class PortfolioContent
    {
        [JsonPropertyName("includeCurrencies")]
        public bool IncludeCurrencies { get; set; }

        [JsonPropertyName("includeMoney")]
        public bool IncludeMoney { get; set; }

        [JsonPropertyName("includePositions")]
        public bool IncludePositions { get; set; }

        [JsonPropertyName("includeMaxBuySell")]
        public bool IncludeMaxBuySell { get; set; }
    }
    public class PositionRow
    {
        [JsonPropertyName("securityCode")]
        public string SecurityCode { get; set; }

        [JsonPropertyName("market")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Market Market { get; set; }

        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }

        [JsonPropertyName("currentPrice")]
        public decimal CurrentPrice { get; set; }

        [JsonPropertyName("equity")]
        public decimal Equity { get; set; }

        [JsonPropertyName("averagePrice")]
        public decimal AveragePrice { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("accumulatedProfit")]
        public decimal AccumulatedProfit { get; set; }

        [JsonPropertyName("todayProfit")]
        public decimal TodayProfit { get; set; }

        [JsonPropertyName("unrealizedProfit")]
        public decimal UnrealizedProfit { get; set; }

        [JsonPropertyName("profit")]
        public decimal Profit { get; set; }

        [JsonPropertyName("maxBuy")]
        public decimal MaxBuy { get; set; }

        [JsonPropertyName("maxSell")]
        public decimal MaxSell { get; set; }

        [JsonPropertyName("priceCurrency")]
        public string PriceCurrency { get; set; }

        [JsonPropertyName("averagePriceCurrency")]
        public string AveragePriceCurrency { get; set; }

        [JsonPropertyName("averageRate")]
        public decimal AverageRate { get; set; }
    }

    public class CurrencyRow
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal CrossRate { get; set; }
        public decimal Equity { get; set; }
        public decimal UnrealizedProfit { get; set; }
    }

    public class MoneyRow
    {
        [JsonPropertyName("market")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Market Market { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }

    public class PortfolioFilter
    {
        public string ClientId { get; set; }
        public bool IncludeCurrencies { get; set; } = true;
        public bool IncludeMoney { get; set; } = true;
        public bool IncludePositions { get; set; } = true;
        public bool IncludeMaxBuySell { get; set; } = true;
    }
}
