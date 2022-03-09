using LanguageExt;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface ICoinService
    {
        IEnumerator<Coin> CoinEnumerator { get; }
        Task<Coin[]> GetCoinsAsync(CancellationToken cancellationToken = default);
        Task<Option<Coin>> GetCoinAsync(string id, CancellationToken cancellationToken = default);
        Task<Option<Coin>> AddCoinAsync(string id, CancellationToken cancellationToken = default);
        Task<Option<Coin>> RemoveCoinAsync(string id, CancellationToken cancellationToken = default);
        Task<BlacklistedCoin[]> GetBlacklistedCoinsAsync(CancellationToken cancellationToken = default);
        Task<Option<BlacklistedCoin>> GetBlacklistedCoinAsync(string id, CancellationToken cancellationToken = default);
        Task<Option<BlacklistedCoin>> BlacklistCoinAsync(
            BlacklistedCoin blacklistedCoin, CancellationToken cancellationToken = default);
        Task<Option<BlacklistedCoin>> RemoveBlacklistedCoinAsync(string id, CancellationToken cancellationToken = default);
    }
}
