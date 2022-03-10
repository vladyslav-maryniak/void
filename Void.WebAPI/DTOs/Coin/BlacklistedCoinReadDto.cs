using System;

namespace Void.WebAPI.DTOs.Coin
{
    public class BlacklistedCoinReadDto
    {
        public string Id { get; set; }
        public string Reason { get; set; }
        public DateTime BlacklistedAt { get; set; }
    }
}
