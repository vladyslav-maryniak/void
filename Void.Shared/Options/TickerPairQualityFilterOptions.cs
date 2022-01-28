namespace Void.Shared.Options
{
    public class TickerPairQualityFilterOptions
    {
        public static string Key => "TickerPairQualityFilter";
        public TickerPairQualityFilter Advanced { get; set; }
        public TickerPairQualityFilter Default { get; set; }
    }

    public class TickerPairQualityFilter
    {
        public double MinProfitPercentage { get; set; }
    }
}
