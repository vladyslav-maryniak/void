namespace Void.Shared.Options.Policies
{
    public class WaitAndRetryOptions
    {
        public double MedianFirstRetryDelay { get; set; }
        public int RetryCount { get; set; }
    }
}