namespace Void.Shared.Options
{
    public class CoinGeckoOptions
    {
        public static string Key => "CoinGecko";
        public string Host { get; set; }
        public string BasePath { get; set; }
        public string Scheme { get; set; }
    }
}
