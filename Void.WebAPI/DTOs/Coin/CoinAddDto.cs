using Void.WebAPI.Validators.Abstractions;

namespace Void.WebAPI.DTOs.Coin
{
    public class CoinAddDto : IIdentifiable<string>
    {
        public string Id { get; set; }
    }
}
