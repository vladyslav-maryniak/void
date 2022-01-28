using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.BackgroundServices
{
    public class CoinGeckoRefreshService : BackgroundService
    {
        private readonly INotifier notifier;
        private readonly IServiceProvider serviceProvider;
        private readonly RefreshOptions refreshOptions;

        private Dictionary<string, DateTime> checkingTimestamps;

        public CoinGeckoRefreshService(
            INotifier notifier,
            IServiceProvider serviceProvider,
            IOptions<RefreshOptions> refreshOptions)
        {
            this.notifier = notifier;
            this.serviceProvider = serviceProvider;
            this.refreshOptions = refreshOptions.Value;

            checkingTimestamps = new();
        }

        private IEnumerator<Coin> CoinEnumerator { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var coinService = scope.ServiceProvider.GetRequiredService<ICoinService>();

            CoinEnumerator = coinService.CoinEnumerator;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (CoinEnumerator.MoveNext())
                {
                    await RefreshTickersAsync(CoinEnumerator.Current.Id, stoppingToken);
                    await CheckCoinTickersAsync(CoinEnumerator.Current.Id, stoppingToken);
                }
                else
                {
                    CoinEnumerator.Dispose();
                    CoinEnumerator = coinService.CoinEnumerator;
                }
                await Task.Delay(TimeSpan.FromMilliseconds(refreshOptions.Delay), stoppingToken);
            }
        }

        public async Task RefreshTickersAsync(string coinId, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var tickerService = scope.ServiceProvider.GetRequiredService<ITickerService>();
            var exchangeService = scope.ServiceProvider.GetRequiredService<IExchangeService>();
            var coinGeckoService = scope.ServiceProvider.GetRequiredService<ICoinGeckoService>();

            var exchanges = await exchangeService.GetExchangesAsync();
            var exchangeIds = exchanges.Select(x => x.Id).ToArray();

            var coinTickers = await coinGeckoService.GetCoinTickersAsync(coinId, exchangeIds, cancellationToken);

            await tickerService.RefreshTickersAsync(coinId, coinTickers, cancellationToken);
        }

        public async Task CheckCoinTickersAsync(string coinId, CancellationToken cancellationToken)
        {
            if (checkingTimestamps.TryGetValue(coinId, out DateTime checkingTime))
            {
                if ((DateTime.Now - checkingTime).TotalMilliseconds < refreshOptions.SendingTimeout)
                {
                    return;
                }
                checkingTimestamps.Remove(coinId);
            }

            using var scope = serviceProvider.CreateScope();
            var tickerPairService = scope.ServiceProvider.GetRequiredService<ITickerPairService>();
            var tickerPair = await tickerPairService.GetTickerPairAsync(coinId, defaultFilters: false, cancellationToken);
            
            if (tickerPair is not null)
            {
                var message = JsonConvert.SerializeObject(tickerPair, Formatting.Indented);
                await notifier.NotifyAsync(message);

                checkingTimestamps[coinId] = DateTime.Now;
            }
        }
    }
}
