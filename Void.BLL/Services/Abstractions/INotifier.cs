using System.Threading.Tasks;

namespace Void.BLL.Services.Abstractions
{
    public interface INotifier
    {
        Task NotifyAsync(string message);
    }
}
