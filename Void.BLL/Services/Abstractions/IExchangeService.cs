using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface IExchangeService
    {
        Task<IEnumerable<Exchange>> GetExchangesAsync(CancellationToken cancellationToken = default);
        Task<Exchange> GetExchangeAsync(string id, CancellationToken cancellationToken = default);
        Task AddExchangeAsync(Exchange coin, CancellationToken cancellationToken = default);
        Task RemoveExchangeAsync(string id, CancellationToken cancellationToken = default);
    }
}
