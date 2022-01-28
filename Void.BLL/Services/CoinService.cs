using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL;
using Void.DAL.Entities;

namespace Void.BLL.Services
{
    public class CoinService : ICoinService
    {
        private readonly VoidContext context;

        public CoinService(VoidContext context)
        {
            this.context = context;
        }

        public IEnumerator<Coin> CoinEnumerator
            => context.Coins
                .AsNoTracking()
                .AsEnumerable()
                .GetEnumerator();

        public async Task<IEnumerable<Coin>> GetCoinsAsync()
            => await context.Coins
                .AsNoTracking()
                .ToArrayAsync();

        public async Task<Coin> GetCoinAsync(string id)
            => await context.Coins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddCoinAsync(Coin coin)
        {
            if (coin is null)
            {
                throw new ArgumentNullException(nameof(coin));
            }

            await context.AddAsync(coin);
            await context.SaveChangesAsync();
        }

        public async Task RemoveCoinAsync(string id)
        {
            var coin = await GetCoinAsync(id);
            if (coin is null)
            {
                throw new ArgumentNullException(nameof(coin));
            }

            context.Remove(coin);
            await context.SaveChangesAsync();
        }
    }
}
