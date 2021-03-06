using Newtonsoft.Json;
using System;

namespace Void.BLL.DTOs.Ticker
{

    public class CoinGeckoTickerDto
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("market")]
        public MarketDto Market { get; set; }

        [JsonProperty("last")]
        public decimal Last { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("cost_to_move_up_usd")]
        public decimal CostToMoveUpUsd { get; set; }

        [JsonProperty("cost_to_move_down_usd")]
        public decimal CostToMoveDownUsd { get; set; }

        [JsonProperty("converted_last")]
        public ConvertedLastDto ConvertedLast { get; set; }

        [JsonProperty("converted_volume")]
        public ConvertedVolumeDto ConvertedVolume { get; set; }

        [JsonProperty("trust_score")]
        public string TrustScore { get; set; }

        [JsonProperty("bid_ask_spread_percentage")]
        public double? BidAskSpreadPercentage { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("last_traded_at")]
        public DateTime LastTradedAt { get; set; }

        [JsonProperty("last_fetch_at")]
        public DateTime LastFetchAt { get; set; }

        [JsonProperty("is_anomaly")]
        public bool IsAnomaly { get; set; }

        [JsonProperty("is_stale")]
        public bool IsStale { get; set; }

        [JsonProperty("trade_url")]
        public string TradeUrl { get; set; }

        [JsonProperty("token_info_url")]
        public string TokenInfoUrl { get; set; }

        [JsonProperty("coin_id")]
        public string CoinId { get; set; }

        [JsonProperty("target_coin_id")]
        public string TargetCoinId { get; set; }
    }
}
