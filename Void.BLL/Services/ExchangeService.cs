using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL;
using Void.DAL.Entities;

namespace Void.BLL.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly VoidContext context;

        public ExchangeService(VoidContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Exchange>> GetExchangesAsync(CancellationToken cancellationToken = default)
            => await context.Exchanges
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

        public async Task<Exchange> GetExchangeAsync(string id, CancellationToken cancellationToken = default)
            => await context.Exchanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task AddExchangeAsync(Exchange exchange, CancellationToken cancellationToken = default)
        {
            await context.AddAsync(exchange,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveExchangeAsync(string id, CancellationToken cancellationToken = default)
        {
            context.Remove(new Exchange { Id = id });
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
