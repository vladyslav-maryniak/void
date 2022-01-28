namespace Void.Shared.Options
{
    public class DiscordOptions
    {
        public static string Key => "Discord";
        public string Token { get; set; }
        public ulong ChannelId { get; set; }
    }
}
