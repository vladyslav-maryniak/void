using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.DTOs.Coin;
using Void.BLL.DTOs.Exchange;
using Void.BLL.DTOs.Ticker;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.Services
{
    public class CoinGeckoProvider : ICryptoDataProvider
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly string baseUri;

        public CoinGeckoProvider(HttpClient httpClient, IOptions<CoinGeckoOptions> options, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.mapper = mapper;
            baseUri = $"{options.Value.Scheme}://{options.Value.Host}{options.Value.BasePath}";
        }

        public async Task<Ticker[]> GetCoinTickersAsync(
            string id, string[] exchangeIds, CancellationToken cancellationToken = default)
        {
            var requestUri = GetCoinTickersUri(id, exchangeIds);
            var coinTickersDto = 
                await GetDeserializedHttpResponseBodyAsync<CoinGeckoCoinTickersReadDto>(requestUri, cancellationToken);
            
            return mapper.Map<Ticker[]>(coinTickersDto.Tickers);
        }

        public async Task<Coin[]> GetSupportedCoinsAsync(CancellationToken cancellationToken = default)
        {
            var requestUri = GetSupportedCoinsUri();
            var coinDtos =
                await GetDeserializedHttpResponseBodyAsync<CoinGeckoCoinReadDto[]>(requestUri, cancellationToken);

            return mapper.Map<Coin[]>(coinDtos);
        }

        public async Task<Coin> GetSupportedCoinAsync(string id, CancellationToken cancellationToken = default)
        {
            var supportedCoins = await GetSupportedCoinsAsync(cancellationToken);
            return supportedCoins.First(x => x.Id == id);
        }

        public async Task<Exchange[]> GetSupportedExchangesAsync(CancellationToken cancellationToken = default)
        {
            var requestUri = GetSupportedExchangesUri();
            var exchangeDtos =
                await GetDeserializedHttpResponseBodyAsync<CoinGeckoExchangeReadDto[]>(requestUri, cancellationToken);

            return mapper.Map<Exchange[]>(exchangeDtos);
        }

        public async Task<Exchange> GetSupportedExchangeAsync(string id, CancellationToken cancellationToken = default)
        {
            var supportedExchanges = await GetSupportedExchangesAsync(cancellationToken);
            return supportedExchanges.First(x => x.Id == id);
        }

        private async Task<T> GetDeserializedHttpResponseBodyAsync<T>(
            Uri requestUri, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(requestUri, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<T>(content);
        }

        private Uri GetCoinTickersUri(string id, string[] exchangeIds)
        {
            var endpoint = $"{baseUri}/coins/{id}/tickers";
            Dictionary<string, string> parameters = new()
            {
                ["depth"] = "true",
                ["exchange_ids"] = string.Join(',', exchangeIds)
            };
            return new (QueryHelpers.AddQueryString(endpoint, parameters));
        }

        private Uri GetSupportedCoinsUri() => new($"{baseUri}/coins/list");

        private Uri GetSupportedExchangesUri() => new($"{baseUri}/exchanges/list");
    }
}
