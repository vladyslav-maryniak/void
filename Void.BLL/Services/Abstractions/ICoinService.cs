using System.Collections.Generic;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface ICoinService
    {
        IEnumerator<Coin> CoinEnumerator { get; }
        Task<IEnumerable<Coin>> GetCoinsAsync();
        Task<Coin> GetCoinAsync(string id);
        Task AddCoinAsync(Coin coin);
        Task RemoveCoinAsync(string id);
    }
}
