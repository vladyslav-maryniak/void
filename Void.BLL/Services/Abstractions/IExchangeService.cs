using LanguageExt;
using System.Threading;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface IExchangeService
    {
        Task<Exchange[]> GetExchangesAsync(CancellationToken cancellationToken = default);
        Task<Option<Exchange>> GetExchangeAsync(string id, CancellationToken cancellationToken = default);
        Task<Option<Exchange>> AddExchangeAsync(string id, CancellationToken cancellationToken = default);
        Task<Option<Exchange>> RemoveExchangeAsync(string id, CancellationToken cancellationToken = default);
    }
}
