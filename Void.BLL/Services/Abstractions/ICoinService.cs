using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface ICoinService
    {
        IEnumerator<Coin> CoinEnumerator { get; }
        Task<IEnumerable<Coin>> GetCoinsAsync(CancellationToken cancellationToken = default);
        Task<Coin> GetCoinAsync(string id, CancellationToken cancellationToken = default);
        Task AddCoinAsync(Coin coin, CancellationToken cancellationToken = default);
        Task RemoveCoinAsync(string id, CancellationToken cancellationToken = default);
    }
}
