using Newtonsoft.Json;

namespace Void.BLL.DTOs.Exchange
{
    public class CoinGeckoExchangeReadDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
