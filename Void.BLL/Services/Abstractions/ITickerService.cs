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
        Task<Ticker> GetTickerAsync(int id);
        Task RemoveTickerAsync(int id);
        Task RefreshTickersAsync(string coinId, ICollection<Ticker> coinTickers, CancellationToken cancellationToken);
        Ticker[] Filter(Ticker[] tickers, TickerFilter filter);
    }
}
