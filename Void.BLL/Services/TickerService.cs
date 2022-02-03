using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Ticker> GetTickerAsync(int id, CancellationToken cancellationToken = default)
            => await context.Tickers
                .AsNoTracking()
                .Include(x => x.Coin)
                .Include(x => x.Exchange)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task RemoveTickerAsync(int id, CancellationToken cancellationToken = default)
        {
            context.Remove(new Ticker { Id = id });
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RefreshTickersAsync(
            string coinId, ICollection<Ticker> coinTickers, CancellationToken cancellationToken = default)
        {
            var obsoleteCoinTickers = await context.Tickers
                .AsNoTracking()
                .Where(x => x.CoinId == coinId)
                .ToArrayAsync(cancellationToken);

            context.RemoveRange(obsoleteCoinTickers);
            await context.AddRangeAsync(coinTickers, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
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
