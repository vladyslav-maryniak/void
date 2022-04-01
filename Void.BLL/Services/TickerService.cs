using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.Services
{
    public class TickerService : ITickerService
    {
        private readonly VoidContext context;

        public TickerService(VoidContext context)
        {
            this.context = context;
        }

        public async Task<Ticker[]> GetTickersAsync(CancellationToken cancellationToken = default)
            => await context.Tickers
                .AsNoTracking()
                .Include(x => x.Coin)
                .Include(x => x.Exchange)
                .ToArrayAsync(cancellationToken);

        public async Task<Ticker[]> GetTickersAsync(string coinId, CancellationToken cancellationToken = default)
            => await context.Tickers
                .AsNoTracking()
                .Where(x => x.CoinId == coinId)
                .Include(x => x.Coin)
                .Include(x => x.Exchange)
                .ToArrayAsync(cancellationToken);

        public async Task<Option<Ticker>> GetTickerAsync(int id, CancellationToken cancellationToken = default)
        {
            var ticker = await context.Tickers
                .AsNoTracking()
                .Include(x => x.Coin)
                .Include(x => x.Exchange)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return ticker is null ? Option<Ticker>.None : ticker.ToSome();
        }

        public async Task RefreshTickersAsync(
            string coinId, ICollection<Ticker> coinTickers, CancellationToken cancellationToken = default)
        {
            var obsoleteCoinTickers = await context.Tickers
                .AsNoTracking()
                .Where(x => x.CoinId == coinId)
                .ToArrayAsync(cancellationToken);

            if (obsoleteCoinTickers.Length > 0)
            {
                context.RemoveRange(obsoleteCoinTickers);
            }

            var targetCoinTickers = coinTickers
                .Where(x => x.CoinId == coinId)
                .ToArray();

            if (targetCoinTickers.Length > 0)
            {
                await context.AddRangeAsync(targetCoinTickers, cancellationToken);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public Ticker[] Filter(Ticker[] tickers, TickerFilter filter)
            => tickers
                .Where(x => x.TargetCoinId == filter.TargetCoinId &&
                            x.BidAskSpreadPercentage < filter.MaxBidAskSpreadPercentage &&
                            x.CostToMoveUpUsd > filter.MinCostToMoveUpUsd &&
                            x.CostToMoveDownUsd > filter.MinCostToMoveDownUsd &&
                            x.IsStale == filter.IsStale)
                .ToArray();
    }
}
