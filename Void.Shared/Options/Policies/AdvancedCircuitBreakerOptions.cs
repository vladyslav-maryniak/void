namespace Void.Shared.Options.Policies
{
    public class AdvancedCircuitBreakerOptions
    {
        public double FailureThreshold { get; set; }
        public double SamplingDuration { get; set; }
        public int MinimumThroughput { get; set; }
        public double DurationOfBreak { get; set; }
    }
}