using Void.BLL.DTOs.Coin;
using Void.BLL.DTOs.Exchange;

namespace Void.BLL.DTOs.Ticker
{
    public class TickerNotificationReadDto
    {
        public ExchangeNotificationReadDto Exchange { get; set; }
        public decimal Last { get; set; }
        public decimal CostToMoveUpUsd { get; set; }
        public decimal CostToMoveDownUsd { get; set; }
    }
}
