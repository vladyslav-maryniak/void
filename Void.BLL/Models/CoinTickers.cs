using Void.DAL.Entities;

namespace Void.BLL.Models
{
    public class CoinTickers
    {
        public string Name { get; set; }
        public Ticker[] Tickers { get; set; }
    }
}
