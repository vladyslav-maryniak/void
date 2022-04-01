using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.DTOs.Ticker;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.BackgroundServices
{
    public class CoinGeckoRefreshService : BackgroundService
    {
        private readonly INotifier notifier;
        private readonly IMapper mapper;
        private readonly IServiceProvider serviceProvider;
        private readonly CoinGeckoOptions coinGeckoOptions;
        private readonly RefreshOptions refreshOptions;

        private Dictionary<string, DateTime> checkingTimestamps;

        public CoinGeckoRefreshService(
            INotifier notifier,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IOptions<RefreshOptions> refreshOptions,
            IOptions<CoinGeckoOptions> coinGeckoOptions)
        {
            this.notifier = notifier;
            this.mapper = mapper;
            this.serviceProvider = serviceProvider;
            this.coinGeckoOptions = coinGeckoOptions.Value;
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
                    try
                    {
                        await RefreshTickersAsync(CoinEnumerator.Current.Id, stoppingToken);
                        await CheckCoinTickersAsync(CoinEnumerator.Current.Id, stoppingToken);
                        
                        await Task.Delay(TimeSpan.FromMilliseconds(refreshOptions.Delay), stoppingToken);
                    }
                    catch (BrokenCircuitException)
                    {
                        await Task.Delay(
                            TimeSpan.FromSeconds(coinGeckoOptions.Policy.AdvancedCircuitBreaker.DurationOfBreak),
                            stoppingToken);
                    }
                    catch (Exception e)
                    {
                        await notifier.NotifyAsync(
                            string.Join(Environment.NewLine,
                                CoinEnumerator.Current.Id,
                                e.Message,
                                e.InnerException?.Message ?? string.Empty,
                                e.ToString()));
                    }
                }
                else
                {
                    CoinEnumerator.Dispose();
                    CoinEnumerator = coinService.CoinEnumerator;
                }
            }
        }

        private async Task RefreshTickersAsync(string coinId, CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
            var tickerService = scope.ServiceProvider.GetRequiredService<ITickerService>();
            var exchangeService = scope.ServiceProvider.GetRequiredService<IExchangeService>();
            var dataProvider = scope.ServiceProvider.GetRequiredService<ICryptoDataProvider>();

            var exchanges = await exchangeService.GetExchangesAsync(cancellationToken);
            var exchangeIds = exchanges.Select(x => x.Id).ToArray();

            var coinTickers = await dataProvider.GetCoinTickersAsync(coinId, exchangeIds, cancellationToken);

            await tickerService.RefreshTickersAsync(coinId, coinTickers, cancellationToken);
        }

        private async Task CheckCoinTickersAsync(string coinId, CancellationToken cancellationToken = default)
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
            var tickerPairOption = await tickerPairService.GetTickerPairAsync(coinId, defaultFilters: false, cancellationToken);

            await tickerPairOption.IfSomeAsync(async tickerPair =>
            {
                var notificationDto = mapper.Map<TickerPairNotificationReadDto>(tickerPair);
                var message = JsonConvert.SerializeObject(notificationDto, Formatting.Indented);
                
                await notifier.NotifyAsync(message);

                checkingTimestamps[coinId] = DateTime.Now;
            });
        }
    }
}
