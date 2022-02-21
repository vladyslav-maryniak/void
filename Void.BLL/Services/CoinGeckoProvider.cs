using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.DTOs.Coin;
using Void.BLL.DTOs.Exchange;
using Void.BLL.DTOs.Ticker;
using Void.BLL.Extensions;
using Void.BLL.Models;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.Services
{
    public class CoinGeckoProvider : ICryptoDataProvider
    {
        private readonly HttpClient httpClient;
        private readonly CoinGeckoOptions coinGeckoOptions;
        private readonly IMapper mapper;

        public CoinGeckoProvider(HttpClient httpClient, IOptions<CoinGeckoOptions> coinGeckoOptions, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.coinGeckoOptions = coinGeckoOptions.Value;
            this.mapper = mapper;
        }

        public async Task<Ticker[]> GetCoinTickersAsync(
            string id, string[] exchangeIds, CancellationToken cancellationToken = default)
        {
            var request = GetCoinTickersRequest(id, exchangeIds);
            var coinTickers = await GetMappedHttpContentAsync<CoinGeckoCoinTickersReadDto, CoinTickers>(request, cancellationToken);
            return coinTickers.Tickers;
        }

        public async Task<Coin[]> GetSupportedCoinsAsync(CancellationToken cancellationToken = default)
        {
            var request = GetSupportedCoinsRequest();
            return await GetMappedHttpContentAsync<CoinGeckoCoinReadDto[], Coin[]>(request, cancellationToken);
        }

        public async Task<Coin> GetSupportedCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            var supportedCoins = await GetSupportedCoinsAsync(cancellationToken);
            return supportedCoins.First(x => x.Id == id);
        }

        public async Task<Exchange[]> GetSupportedExchangesAsync(CancellationToken cancellationToken = default)
        {
            var request = GetSupportedExchangesRequest();
            return await GetMappedHttpContentAsync<CoinGeckoExchangeReadDto[], Exchange[]>(request, cancellationToken);
        }

        public async Task<Exchange> GetSupportedExchangeAsync(string id, CancellationToken cancellationToken = default)
        {
            var supportedExchanges = await GetSupportedExchangesAsync(cancellationToken);
            return supportedExchanges.First(x => x.Id == id);
        }

        private async Task<TDestination> GetMappedHttpContentAsync<TSource, TDestination>(
            HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.SendAsync(request, cancellationToken);
            var content = await response.Content.ReadAsAsync<TSource>(cancellationToken);
            return mapper.Map<TDestination>(content);
        }

        private HttpRequestMessage GetCoinTickersRequest(string id, string[] exchangeIds)
        {
            var httpMethod = HttpMethod.Get;
            var path = $"/coins/{id}/tickers";
            var query = new NameValueCollection
            {
                { "depth", "true" },
                { "exchange_ids", string.Join(',', exchangeIds) }
            }.ToQueryString();

            return CreateHttpRequest(httpMethod, path, query);
        }

        private HttpRequestMessage GetSupportedCoinsRequest()
        {
            var httpMethod = HttpMethod.Get;
            var path = $"/coins/list";
            return CreateHttpRequest(httpMethod, path);
        }

        private HttpRequestMessage GetSupportedExchangesRequest()
        {
            var httpMethod = HttpMethod.Get;
            var path = $"/exchanges/list";
            return CreateHttpRequest(httpMethod, path);
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod httpMethod, string path, string query = default)
        {
            path = coinGeckoOptions.ApiPrefix + path;
            
            if (string.IsNullOrEmpty(query))
            {
                return new(httpMethod, path);
            }
            return new(httpMethod, $"{path}?{query}");
        }
    }
}
