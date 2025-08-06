using System.Text.Json.Serialization;

namespace FinamAPI.Models
{
    public class Order
    {
        [JsonPropertyName("orderNo")]
        public long OrderNo { get; set; }

        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }

        [JsonPropertyName("securityCode")]
        public string? SecurityCode { get; set; }

        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }

        [JsonPropertyName("buySell")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BuySell BuySell { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("price")]
        public DecimalValue Price { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("condition")]
        public OrderCondition Condition { get; set; }

        [JsonPropertyName("validBefore")]
        public OrderValidBeforeType ValidBefore { get; set; }

        [JsonPropertyName("acceptedAt")]
        public DateTime AcceptedAt { get; set; }

        [JsonPropertyName("securityBoard")]
        public string? SecurityBoard { get; set; }

        [JsonPropertyName("market")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Market Market { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderConditionType
    {
        [JsonPropertyName("Bid")]
        Bid,
        [JsonPropertyName("BidOrLast")]
        BidOrLast,
        [JsonPropertyName("Ask")]
        Ask,
        [JsonPropertyName("AskOrLast")]
        AskOrLast,
        [JsonPropertyName("Time")]
        Time,
        [JsonPropertyName("CovDown")]
        CovDown,
        [JsonPropertyName("CovUp")]
        CovUp,
        [JsonPropertyName("LastUp")]
        LastUp,
        [JsonPropertyName("LastDown")]
        LastDown
    }

    public class OrderCondition
    {
        [JsonPropertyName("type")]
        public OrderConditionType Type { get; set; }

        [JsonPropertyName("price")]
        public DecimalValue Price { get; set; }

        [JsonPropertyName("time")]
        public DateTime? Time { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        [JsonPropertyName("None")]
        None,
        [JsonPropertyName("Active")]
        Active,
        [JsonPropertyName("Matched")]
        Matched,
        [JsonPropertyName("Cancelled")]
        Cancelled
    }
    public class OrderRequest
    {
        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }

        [JsonPropertyName("securityCode")]
        public string? SecurityCode { get; set; }

        [JsonPropertyName("board")]
        public string? Board { get; set; }

        [JsonPropertyName("market")]
        public Market Market { get; set; }

        [JsonPropertyName("buySell")]
        public BuySell BuySell { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public DecimalValue Price { get; set; }

        [JsonPropertyName("property")]
        public string? Property { get; set; } = "PutInQueue";

        [JsonPropertyName("validBefore")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrderValidBeforeType? ValidBefore { get; set; }
    }

    public class OrderResponse
    {
        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }

        [JsonPropertyName("orderNo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? OrderNo { get; set; }
    }


    public class CancelOrderRequest
    {
        public string ClientId { get; set; }
        public long TransactionId { get; set; }
    }
    public class StopOrderRequest : OrderRequest
    {
        [JsonPropertyName("condition")]
        public string? Condition { get; set; }

        [JsonPropertyName("stopPrice")]
        public DecimalValue StopPrice { get; set; }

        [JsonPropertyName("activationPrice")]
        public DecimalValue ActivationPrice { get; set; }

        [JsonPropertyName("expiryTime")]
        public DateTime? ExpiryTime { get; set; }
    }
}

