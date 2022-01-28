namespace Void.Shared.Options
{
    public class TickerFilterOptions
    {
        public static string Key => "TickerFilter";
        public TickerFilter Advanced { get; set; }
        public TickerFilter Default { get; set; }
    }

    public class TickerFilter
    {
        public string TargetCoinId { get; set; }
        public double MaxBidAskSpreadPercentage { get; set; }
        public decimal MinCostToMoveUpUsd { get; set; }
        public decimal MinCostToMoveDownUsd { get; set; }
        public bool IsStale { get; set; }
    }
}
