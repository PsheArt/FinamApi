using FinamAPI.Models;

namespace FinamAPI.Helpers
{
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
}
