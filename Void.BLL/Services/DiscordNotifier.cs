using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Wrap;
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
        private readonly DiscordOptions options;
        private IMessageChannel discordChannel;
        private readonly AsyncPolicyWrap policyWrap;
        private bool isDisposed = false;

        public DiscordNotifier(
            ILogger<DiscordNotifier> logger,
            DiscordSocketClient discordClient,
            IOptions<DiscordOptions> discordOptions)
        {
            this.logger = logger;
            this.discordClient = discordClient;
            options = discordOptions.Value;
            
            var waitAndRetryPolicy = Policy.Handle<HttpException>()
                .WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(
                        TimeSpan.FromSeconds(options.Policy.WaitAndRetry.MedianFirstRetryDelay),
                    options.Policy.WaitAndRetry.RetryCount));

            var breakerPolicy = Policy.Handle<HttpException>()
                .AdvancedCircuitBreakerAsync(
                    options.Policy.AdvancedCircuitBreaker.FailureThreshold,
                    TimeSpan.FromSeconds(options.Policy.AdvancedCircuitBreaker.SamplingDuration),
                    options.Policy.AdvancedCircuitBreaker.MinimumThroughput,
                    TimeSpan.FromSeconds(options.Policy.AdvancedCircuitBreaker.DurationOfBreak));

            policyWrap = Policy.WrapAsync(waitAndRetryPolicy, breakerPolicy);
        }

        public async Task NotifyAsync(string message)
        {
            await policyWrap.ExecuteAsync(async () =>
            {
                if (discordClient.ConnectionState == ConnectionState.Disconnected)
                {
                    await ConnectAsync();
                }
                await discordChannel.SendMessageAsync(message);
            });
        }

        private async Task ConnectAsync()
        {
            discordClient.Log += DiscordClient_Log;
            await discordClient.LoginAsync(TokenType.Bot, options.Token);
            await discordClient.StartAsync();
            discordChannel = await discordClient.GetChannelAsync(options.ChannelId) as IMessageChannel;
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
                await policyWrap.ExecuteAsync(async () =>
                {
                    await DisconnectAsync().ConfigureAwait(false);
                });

                isDisposed = true;
            }
        }
    }
}
