using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Models;

namespace Void.BLL.Services.Abstractions
{
    public interface ITickerPairService
    {
        Task<TickerPair> GetTickerPairAsync(
            string coinId, bool defaultFilters = false, CancellationToken cancellationToken = default);
    }
}