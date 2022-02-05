using Newtonsoft.Json;

namespace Void.BLL.DTOs.Ticker
{
    public class MarketDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("has_trading_incentive")]
        public bool HasTradingIncentive { get; set; }
    }
}
