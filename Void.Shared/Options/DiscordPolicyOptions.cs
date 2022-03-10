using Void.Shared.Options.Policies;

namespace Void.Shared.Options
{
    public class DiscordPolicyOptions
    {
        public WaitAndRetryOptions WaitAndRetry { get; set; }
        public AdvancedCircuitBreakerOptions AdvancedCircuitBreaker { get; set; }
    }
}
