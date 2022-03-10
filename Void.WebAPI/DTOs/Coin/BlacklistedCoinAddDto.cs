using Void.WebAPI.Validators.Abstractions;

namespace Void.WebAPI.DTOs.Coin
{
    public class BlacklistedCoinAddDto : IIdentifiable<string>
    {
        public string Id { get; set; }
        public string Reason { get; set; }
    }
}
