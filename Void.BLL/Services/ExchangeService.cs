using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICryptoDataProvider dataProvider;

        public ExchangeService(VoidContext context, ICryptoDataProvider dataProvider)
        {
            this.context = context;
            this.dataProvider = dataProvider;
        }

        public async Task<Exchange[]> GetExchangesAsync(CancellationToken cancellationToken = default)
            => await context.Exchanges
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

        public async Task<Option<Exchange>> GetExchangeAsync(string id, CancellationToken cancellationToken = default)
        {
            var exchange = await context.Exchanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return exchange?.ToSome() ?? Option<Exchange>.None;
        }

        public async Task<Option<Exchange>> AddExchangeAsync(string id, CancellationToken cancellationToken = default)
        {
            if (await context.Exchanges.AsQueryable().AnyAsync(x => x.Id == id, cancellationToken))
            {
                return Option<Exchange>.None;
            }

            var exchange = await dataProvider.GetSupportedExchangeAsync(id, cancellationToken);

            await context.AddAsync(exchange, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return exchange.ToSome();
        }

        public async Task<Option<Exchange>> RemoveExchangeAsync(string id, CancellationToken cancellationToken = default)
        {
            var exchangeOption = await GetExchangeAsync(id, cancellationToken);

            await exchangeOption.IfSomeAsync(async exchange =>
            {
                context.Remove(exchange);
                await context.SaveChangesAsync(cancellationToken);
            });

            return exchangeOption;
        }
    }
}
