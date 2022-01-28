using Void.Shared.DTOs.Ticker;
using Void.Shared.DTOs.TickerPairQuality;

namespace Void.Shared.DTOs.TickerPair
{
    public class TickerPairReadDto
    {
        public TickerReadDto Demand { get; set; }
        public TickerReadDto Supply { get; set; }
        public TickerPairQualityReadDto Quality { get; set; }
    }
}
