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
using Void.BLL.DTOs.Ticker;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.Shared.Options;

namespace Void.BLL.Services
{
    public class CoinGeckoService : ICoinGeckoService
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly string baseUri;

        public CoinGeckoService(HttpClient httpClient, IOptions<CoinGeckoOptions> options, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.mapper = mapper;
            baseUri = $"{options.Value.Schemes.First()}://{options.Value.Host}{options.Value.BasePath}";
        }

        public async Task<Ticker[]> GetCoinTickersAsync(string id, string[] exchangeIds, CancellationToken cancellationToken = default)
        {
            var endpoint = $"{baseUri}/coins/{id}/tickers";
            var parameters = InitializeParams(exchangeIds);
            var uri = new Uri(QueryHelpers.AddQueryString(endpoint, parameters));

            var response = await httpClient.GetAsync(uri, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var coinTickerDto = JsonConvert.DeserializeObject<CoinGeckoTickerReadDto>(content);
            return mapper.Map<Ticker[]>(coinTickerDto.Tickers);
        }

        private Dictionary<string, string> InitializeParams(string[] exchangeIds)
        {
            var parameters = new Dictionary<string, string>();

            parameters["depth"] = "true";
            parameters["exchange_ids"] = string.Join(',', exchangeIds);

            return parameters;
        }
    }
}
