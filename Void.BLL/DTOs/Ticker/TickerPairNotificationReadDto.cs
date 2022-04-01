using Void.BLL.DTOs.Coin;

namespace Void.BLL.DTOs.Ticker
{
    public class TickerPairNotificationReadDto
    {
        public CoinNotificationReadDto Coin { get; set; }
        public TickerNotificationReadDto Demand { get; set; }
        public TickerNotificationReadDto Supply { get; set; }
        public double ProfitPercentage { get; set; }
    }
}
