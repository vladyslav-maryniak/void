using Newtonsoft.Json;

namespace Void.BLL.DTOs.Ticker
{
    public class ConvertedVolumeDto
    {
        [JsonProperty("btc")]
        public decimal Btc { get; set; }

        [JsonProperty("eth")]
        public decimal Eth { get; set; }

        [JsonProperty("usd")]
        public decimal Usd { get; set; }
    }
}
