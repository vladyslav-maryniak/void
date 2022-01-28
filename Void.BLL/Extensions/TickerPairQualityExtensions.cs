using Void.BLL.Models;
using Void.Shared.Options;

namespace Void.BLL.Extensions
{
    public static class TickerPairQualityExtensions
    {
        public static bool IsValid(this TickerPairQuality quality, TickerPairQualityFilter filter)
            => quality.ProfitPercentage > filter.MinProfitPercentage;
    }
}
