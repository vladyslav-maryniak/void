using Newtonsoft.Json;

namespace Void.BLL.DTOs.Ticker
{
    public class CoinGeckoCoinTickersReadDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tickers")]
        public CoinGeckoTickerDto[] Tickers { get; set; }
    }
}
