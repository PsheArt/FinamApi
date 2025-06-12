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
        public Market Market { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; }

        [JsonPropertyName("lotSize")]
        public int LotSize { get; set; }

        [JsonPropertyName("minStep")]
        public DecimalValue MinStep { get; set; }

        [JsonIgnore]
        public decimal MinStepDecimal => MinStep.ToDecimal();
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
}
