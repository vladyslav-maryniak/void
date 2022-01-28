using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Exchange>> GetExchangesAsync()
            => await context.Exchanges
                .AsNoTracking()
                .ToArrayAsync();

        public async Task<Exchange> GetExchangeAsync(string id)
            => await context.Exchanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddExchangeAsync(Exchange exchange)
        {
            if (exchange is null)
            {
                throw new ArgumentNullException(nameof(exchange));
            }

            await context.AddAsync(exchange);
            await context.SaveChangesAsync();
        }

        public async Task RemoveExchangeAsync(string id)
        {
            var exchange = await GetExchangeAsync(id);
            if (exchange is null)
            {
                throw new ArgumentNullException(nameof(exchange));
            }

            context.Remove(exchange);
            await context.SaveChangesAsync();
        }
    }
}
