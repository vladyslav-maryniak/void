using Newtonsoft.Json;

namespace Void.BLL.DTOs.Coin
{
    public class CoinGeckoCoinReadDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
