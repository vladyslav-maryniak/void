using Microsoft.EntityFrameworkCore;
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

        public CoinService(VoidContext context)
        {
            this.context = context;
        }

        public IEnumerator<Coin> CoinEnumerator
            => context.Coins
                .AsNoTracking()
                .AsEnumerable()
                .GetEnumerator();

        public async Task<IEnumerable<Coin>> GetCoinsAsync(CancellationToken cancellationToken = default)
            => await context.Coins
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

        public async Task<Coin> GetCoinAsync(string id, CancellationToken cancellationToken = default)
            => await context.Coins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task AddCoinAsync(Coin coin, CancellationToken cancellationToken = default)
        {
            await context.AddAsync(coin, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            context.Remove(new Coin { Id = id });
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
