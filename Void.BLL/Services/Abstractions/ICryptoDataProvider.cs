using System.Threading;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface ICryptoDataProvider
    {
        Task<Ticker[]> GetCoinTickersAsync(
            string id, string[] exchangeIds, CancellationToken cancellationToken = default);
        Task<Coin[]> GetSupportedCoinsAsync(CancellationToken cancellationToken = default);
        Task<Coin> GetSupportedCoinAsync(string id, CancellationToken cancellationToken = default);
        Task<Exchange[]> GetSupportedExchangesAsync(CancellationToken cancellationToken = default);
        Task<Exchange> GetSupportedExchangeAsync(string id, CancellationToken cancellationToken = default);
    }
}