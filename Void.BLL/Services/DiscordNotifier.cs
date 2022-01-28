using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.Shared.Options;

namespace Void.BLL.Services
{
    public class DiscordNotifier : INotifier, IAsyncDisposable
    {
        private readonly ILogger<DiscordNotifier> logger;
        private readonly DiscordSocketClient discordClient;
        private readonly DiscordOptions discordOptions;
        private IMessageChannel discordChannel;
        private bool isDisposed = false;

        public DiscordNotifier(
            ILogger<DiscordNotifier> logger,
            DiscordSocketClient discordClient,
            IOptions<DiscordOptions> discordOptions)
        {
            this.logger = logger;
            this.discordClient = discordClient;
            this.discordOptions = discordOptions.Value;
        }

        public async Task NotifyAsync(string message)
        {
            if (discordClient.ConnectionState == ConnectionState.Disconnected)
            {
                await ConnectAsync();
            }
            await discordChannel.SendMessageAsync(message);
        }

        private async Task ConnectAsync()
        {
            discordClient.Log += DiscordClient_Log;
            await discordClient.LoginAsync(TokenType.Bot, discordOptions.Token);
            await discordClient.StartAsync();
            discordChannel = await discordClient.GetChannelAsync(discordOptions.ChannelId) as IMessageChannel;
        }

        private async Task DisconnectAsync()
        {
            await discordClient.LogoutAsync();
            await discordClient.StopAsync();
            discordClient.Log -= DiscordClient_Log;
        }

        private Task DiscordClient_Log(LogMessage arg)
        {
            logger.LogInformation(arg.Exception, arg.Message);
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (!isDisposed)
            {
                await DisconnectAsync().ConfigureAwait(false);
                isDisposed = true;
            }
        }
    }
}
