using Void.DAL.Entities;

namespace Void.BLL.Models
{
    public class TickerPair
    {
        public Ticker Demand { get; set; }
        public Ticker Supply { get; set; }
        public TickerPairQuality Quality { get; set; }
    }
}
