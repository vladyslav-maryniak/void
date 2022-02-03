using System.Threading;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface ICoinGeckoService
    {
        Task<Ticker[]> GetCoinTickersAsync(string id, string[] exchangeIds, CancellationToken cancellationToken = default);
    }
}