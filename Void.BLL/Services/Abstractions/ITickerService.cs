using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.Services.Abstractions
{
    public interface ITickerService
    {
        Task<Ticker[]> GetTickersAsync(CancellationToken cancellationToken = default);
        Task<Ticker[]> GetTickersAsync(string coinId, CancellationToken cancellationToken = default);
        Task<Ticker> GetTickerAsync(int id, CancellationToken cancellationToken = default);
        Task RemoveTickerAsync(int id, CancellationToken cancellationToken = default);
        Task RefreshTickersAsync(string coinId, ICollection<Ticker> coinTickers, CancellationToken cancellationToken = default);
        Ticker[] Filter(Ticker[] tickers, TickerFilter filter);
    }
}
