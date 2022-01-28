namespace Void.Shared.Options
{
    public class RefreshOptions
    {
        public static string Key => "Refresh";
        public double Delay { get; set; }
        public double SendingTimeout { get; set; }
    }
}
