using Void.Shared.Options.Policies;

namespace Void.Shared.Options
{
    public class CoinGeckoPolicyOptions
    {
        public WaitAndRetryOptions WaitAndRetry { get; set; }
        public AdvancedCircuitBreakerOptions AdvancedCircuitBreaker { get; set; }
    }
}
