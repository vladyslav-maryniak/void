namespace Void.Shared.DTOs.Ticker
{
    public class TickerReadDto
    {
        public int Id { get; set; }
        public string CoinId { get; set; }
        public string TargetCoinId { get; set; }
        public decimal Last { get; set; }
        public decimal CostToMoveUpUsd { get; set; }
        public decimal CostToMoveDownUsd { get; set; }
        public string TrustScore { get; set; }
        public double BidAskSpreadPercentage { get; set; }
        public bool IsStale { get; set; }
        public string TradeUrl { get; set; }
    }
}
