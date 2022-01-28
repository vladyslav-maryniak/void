using System.Collections.Generic;
using System.Threading.Tasks;
using Void.DAL.Entities;

namespace Void.BLL.Services.Abstractions
{
    public interface IExchangeService
    {
        Task<IEnumerable<Exchange>> GetExchangesAsync();
        Task<Exchange> GetExchangeAsync(string id);
        Task AddExchangeAsync(Exchange coin);
        Task RemoveExchangeAsync(string id);
    }
}
