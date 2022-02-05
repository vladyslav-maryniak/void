using Void.WebAPI.DTOs.Ticker;
using Void.WebAPI.DTOs.TickerPairQuality;

namespace Void.WebAPI.DTOs.TickerPair
{
    public class TickerPairReadDto
    {
        public TickerReadDto Demand { get; set; }
        public TickerReadDto Supply { get; set; }
        public TickerPairQualityReadDto Quality { get; set; }
    }
}
