namespace Void.Shared.Options
{
    public class CoinGeckoOptions
    {
        public static string Key => "CoinGecko";
        public string Host { get; set; }
        public string ApiPrefix { get; set; }
        public string Scheme { get; set; }
        public CoinGeckoPolicyOptions Policy { get; set; }
    }
}
