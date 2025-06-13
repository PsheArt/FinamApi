using System.Text.Json.Serialization;

namespace FinamAPI.Models
{
    public class Stop
    {
        [JsonPropertyName("stopId")]
        public long StopId { get; set; }

        [JsonPropertyName("securityCode")]
        public string SecurityCode { get; set; }

        [JsonPropertyName("securityBoard")]
        public string SecurityBoard { get; set; }

        [JsonPropertyName("market")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Market Market { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("buySell")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BuySell BuySell { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonPropertyName("linkOrder")]
        public long LinkOrder { get; set; }

        [JsonPropertyName("validBefore")]
        public OrderValidBeforeType ValidBefore { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StopStatus Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("orderNo")]
        public long OrderNo { get; set; }

        [JsonPropertyName("tradeNo")]
        public long TradeNo { get; set; }

        [JsonPropertyName("acceptedAt")]
        public DateTime AcceptedAt { get; set; }

        [JsonPropertyName("canceledAt")]
        public DateTime? CanceledAt { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("takeProfitExtremum")]
        public decimal TakeProfitExtremum { get; set; }

        [JsonPropertyName("takeProfitLevel")]
        public decimal TakeProfitLevel { get; set; }

        [JsonPropertyName("stopLoss")]
        public StopLoss StopLoss { get; set; }

        [JsonPropertyName("takeProfit")]
        public TakeProfit TakeProfit { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StopStatus
    {
        [JsonPropertyName("Active")]
        Active,
        [JsonPropertyName("Executed")]
        Executed,
        [JsonPropertyName("Cancelled")]
        Cancelled
    }

    public class StopRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("securityBoard")]
        public string SecurityBoard { get; set; }

        [JsonPropertyName("securityCode")]
        public string SecurityCode { get; set; }

        [JsonPropertyName("buySell")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BuySell BuySell { get; set; }

        [JsonPropertyName("stopLoss")]
        public StopLoss StopLoss { get; set; }

        [JsonPropertyName("takeProfit")]
        public TakeProfit TakeProfit { get; set; }

        [JsonPropertyName("expirationDate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ExpirationDate { get; set; }

        [JsonPropertyName("linkOrder")]
        public long LinkOrder { get; set; }

        [JsonPropertyName("validBefore")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrderValidBeforeType ValidBefore { get; set; }
    }

    public class StopLoss
    {
        [JsonPropertyName("activationPrice")]
        public decimal ActivationPrice { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("marketPrice")]
        public bool MarketPrice { get; set; }

        [JsonPropertyName("quantity")]
        public StopQuantity Quantity { get; set; }

        [JsonPropertyName("time")]
        public int Time { get; set; }

        [JsonPropertyName("useCredit")]
        public bool UseCredit { get; set; }
    }

    public class TakeProfit
    {
        [JsonPropertyName("activationPrice")]
        public decimal ActivationPrice { get; set; }

        [JsonPropertyName("correctionPrice")]
        public StopPrice CorrectionPrice { get; set; }

        [JsonPropertyName("spreadPrice")]
        public StopPrice SpreadPrice { get; set; }

        [JsonPropertyName("marketPrice")]
        public bool MarketPrice { get; set; }

        [JsonPropertyName("quantity")]
        public StopQuantity Quantity { get; set; }

        [JsonPropertyName("time")]
        public int Time { get; set; }

        [JsonPropertyName("useCredit")]
        public bool UseCredit { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StopQuantityUnit
    {
        [JsonPropertyName("Percent")]
        Percent,
        [JsonPropertyName("Lots")]
        Lots
    }

    public class StopQuantity
    {
        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("units")]
        public StopQuantityUnit Units { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StopPriceUnit
    {
        [JsonPropertyName("Percent")]
        Percent,
        [JsonPropertyName("Pips")]
        Pips
    }

    public class StopPrice
    {
        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("units")]
        public StopPriceUnit Units { get; set; }
    }

    public class CancelStopRequest
    {
        public string ClientId { get; set; }
        public long StopId { get; set; }
    }

    public class StopResponse
    {
        public long TransactionId { get; set; }
        public long StopId { get; set; }
    }
}
