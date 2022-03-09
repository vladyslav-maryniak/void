using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL;
using Void.DAL.Entities;

namespace Void.BLL.Services
{
    public class CoinService : ICoinService
    {
        private readonly VoidContext context;
        private readonly ICryptoDataProvider dataProvider;

        public CoinService(VoidContext context, ICryptoDataProvider dataProvider)
        {
            this.context = context;
            this.dataProvider = dataProvider;
        }

        public IEnumerator<Coin> CoinEnumerator
            => context.Coins
                .AsNoTracking()
                .AsEnumerable()
                .GetEnumerator();

        public async Task<Coin[]> GetCoinsAsync(CancellationToken cancellationToken = default)
            => await context.Coins
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

        public async Task<Option<Coin>> GetCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            var coin = await context.Coins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return coin?.ToSome() ?? Option<Coin>.None;
        }

        public async Task<Option<Coin>> AddCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            if (await context.Coins.AsQueryable().AnyAsync(x => x.Id == id, cancellationToken))
            {
                return Option<Coin>.None;
            }

            var coin = await dataProvider.GetSupportedCoinAsync(id, cancellationToken);
            
            await context.AddAsync(coin, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            return coin.ToSome();
        }

        public async Task<Option<Coin>> RemoveCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            var coinOption = await GetCoinAsync(id, cancellationToken);

            await coinOption.IfSomeAsync(async coin =>
            {
                context.Remove(coin);
                await context.SaveChangesAsync(cancellationToken);
            });

            return coinOption;
        }

        public async Task<BlacklistedCoin[]> GetBlacklistedCoinsAsync(CancellationToken cancellationToken = default)
            => await context.BlacklistedCoins
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

        public async Task<Option<BlacklistedCoin>> GetBlacklistedCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            var coin = await context.BlacklistedCoins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return coin?.ToSome() ?? Option<BlacklistedCoin>.None;
        }

        public async Task<Option<BlacklistedCoin>> BlacklistCoinAsync(
            BlacklistedCoin blacklistedCoin, CancellationToken cancellationToken = default)
        {
            if (await context.BlacklistedCoins.AsQueryable().AnyAsync(x => x.Id == blacklistedCoin.Id, cancellationToken))
            {
                return Option<BlacklistedCoin>.None;
            }

            blacklistedCoin.BlacklistedAt = DateTime.UtcNow;

            await context.AddAsync(blacklistedCoin, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return blacklistedCoin.ToSome();
        }

        public async Task<Option<BlacklistedCoin>> RemoveBlacklistedCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            var coinOption = await GetBlacklistedCoinAsync(id, cancellationToken);

            await coinOption.IfSomeAsync(async coin =>
            {
                context.Remove(coin);
                await context.SaveChangesAsync(cancellationToken);
            });

            return coinOption;
        }
    }
}
