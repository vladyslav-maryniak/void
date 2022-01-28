using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Extensions;
using Void.BLL.Models;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.Services
{
    public class TickerPairService : ITickerPairService
    {
        private readonly ITickerService tickerService;
        private readonly TickerFilterOptions tickerFilterOptions;
        private readonly TickerPairQualityFilterOptions tickerPairQualityFilterOptions;

        public TickerPairService(
            ITickerService tickerService,
            IOptions<TickerFilterOptions> tickerFilterOptions,
            IOptions<TickerPairQualityFilterOptions> tickerPairQualityFilterOptions)
        {
            this.tickerService = tickerService;
            this.tickerFilterOptions = tickerFilterOptions.Value;
            this.tickerPairQualityFilterOptions = tickerPairQualityFilterOptions.Value;
        }

        public async Task<TickerPair> GetTickerPairAsync(
            string coinId, bool defaultFilters = true, CancellationToken cancellationToken = default)
        {
            var coinTickers = await tickerService.GetTickersAsync(coinId, cancellationToken);
            
            var tickerFilter = defaultFilters ?
                tickerFilterOptions.Default :
                tickerFilterOptions.Advanced;
            var tickerPairQualityFilter = defaultFilters ?
                tickerPairQualityFilterOptions.Default :
                tickerPairQualityFilterOptions.Advanced;

            coinTickers = tickerService.Filter(coinTickers, tickerFilter);

            if (TryGetTickerPair(coinTickers, out TickerPair tickerPair))
            {
                if (tickerPair.Quality.IsValid(tickerPairQualityFilter))
                {
                    return tickerPair;
                }
            }
            return null;
        }

        private bool TryGetTickerPair(Ticker[] tickers, out TickerPair tickerPair)
        {
            if (tickers.Length < 2)
            {
                tickerPair = null;
                return false;
            }

            tickerPair = new()
            {
                Demand = tickers.OrderBy(x => x.Last)
                                .First(),
                Supply = tickers.OrderByDescending(x => x.Last)
                                .First()
            };
            tickerPair.Quality = new()
            {
                ProfitPercentage = GetProfitPercentage(tickerPair)
            };

            return true;
        }

        private double GetProfitPercentage(TickerPair tickerPair)
            => (double)(tickerPair.Supply.Last / tickerPair.Demand.Last * 100) - 100;
    }
}
