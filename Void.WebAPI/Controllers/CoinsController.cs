using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.WebAPI.Contracts;
using Void.WebAPI.DTOs.Coin;

namespace Void.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly ICoinService coinService;
        private readonly IMapper mapper;

        public CoinsController(ICoinService coinService, IMapper mapper)
        {
            this.coinService = coinService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CoinReadDto[]>> GetCoinsAsync(CancellationToken cancellationToken)
        {
            var coins = await coinService.GetCoinsAsync(cancellationToken);
            return Ok(mapper.Map<CoinReadDto[]>(coins));
        }

        [HttpGet("{id:regex(^([[a-z0-9]]*)(-[[a-z0-9]]+)*$):length(1,55)}", Name = "GetCoinAsync")]
        public async Task<ActionResult<CoinReadDto>> GetCoinAsync(string id, CancellationToken cancellationToken)
        {
            var coinOption = await coinService.GetCoinAsync(id, cancellationToken);
            return coinOption.Match<ActionResult<CoinReadDto>>(coin =>
                Ok(mapper.Map<CoinReadDto>(coin)), NotFound());
        }

        [HttpPost]
        public async Task<ActionResult<CoinReadDto>> AddCoinAsync(
            CoinAddDto coinDto, CancellationToken cancellationToken)
        {
            var coinOption = await coinService.AddCoinAsync(coinDto.Id, cancellationToken);
            return coinOption.Match<ActionResult<CoinReadDto>>(
                coin =>
                {
                    var coinDto = mapper.Map<CoinReadDto>(coin);
                    return CreatedAtRoute(nameof(GetCoinAsync), new { coin.Id }, coinDto);
                },
                () =>
                {
                    ValidationErrorResponse error = new();
                    ValidationErrorModel model = new()
                    {
                        PropertyName = nameof(coinDto.Id),
                        Message = "Coin with provided 'id' has already been added"
                    };
                    error.Errors.Add(model);

                    return BadRequest(error);
                });
        }

        [HttpDelete("{id:regex(^([[a-z0-9]]*)(-[[a-z0-9]]+)*$):length(1,55)}")]
        public async Task<ActionResult> RemoveCoinAsync(string id, CancellationToken cancellationToken)
        {
            var coinOption = await coinService.RemoveCoinAsync(id, cancellationToken);
            return coinOption.Match<ActionResult>(_ => NoContent(), NotFound());
        }

        [HttpGet("blacklist")]
        public async Task<ActionResult<BlacklistedCoinReadDto[]>> GetBlacklistedCoinsAsync(CancellationToken cancellationToken)
        {
            var coins = await coinService.GetBlacklistedCoinsAsync(cancellationToken);
            return Ok(mapper.Map<BlacklistedCoinReadDto[]>(coins));
        }

        [HttpGet("blacklist/{id:regex(^([[a-z0-9]]*)(-[[a-z0-9]]+)*$):length(1,55)}", Name = "GetBlacklistedCoinAsync")]
        public async Task<ActionResult<BlacklistedCoinReadDto>> GetBlacklistedCoinAsync(string id, CancellationToken cancellationToken)
        {
            var coinOption = await coinService.GetBlacklistedCoinAsync(id, cancellationToken);
            return coinOption.Match<ActionResult<BlacklistedCoinReadDto>>(coin =>
                Ok(mapper.Map<BlacklistedCoinReadDto>(coin)), NotFound());
        }

        [HttpPost("blacklist")]
        public async Task<ActionResult<BlacklistedCoinReadDto>> BlacklistCoinAsync(
            BlacklistedCoinAddDto coinDto, CancellationToken cancellationToken)
        {
            var blacklistedCoin = mapper.Map<BlacklistedCoin>(coinDto);
            var coinOption = await coinService.BlacklistCoinAsync(blacklistedCoin, cancellationToken);
            return await coinOption.MatchAsync<BlacklistedCoin, ActionResult<BlacklistedCoinReadDto>>(
                async coin =>
                {
                    await coinService.RemoveCoinAsync(coin.Id, cancellationToken);

                    var coinDto = mapper.Map<BlacklistedCoinReadDto>(coin);
                    return CreatedAtRoute(nameof(GetBlacklistedCoinAsync), new { coin.Id }, coinDto);
                },
                () =>
                {
                    ValidationErrorResponse error = new();
                    ValidationErrorModel model = new()
                    {
                        PropertyName = nameof(coinDto.Id),
                        Message = "Coin with provided 'id' has already been blacklisted"
                    };
                    error.Errors.Add(model);

                    return BadRequest(error);
                });
        }

        [HttpDelete("blacklist/{id:regex(^([[a-z0-9]]*)(-[[a-z0-9]]+)*$):length(1,55)}")]
        public async Task<ActionResult<BlacklistedCoinReadDto>> RemoveBlacklistedCoinAsync(string id, CancellationToken cancellationToken)
        {
            var coinOption = await coinService.RemoveBlacklistedCoinAsync(id, cancellationToken);
            return await coinOption.MatchAsync<BlacklistedCoin, ActionResult<BlacklistedCoinReadDto>>(
                async coin =>
                {
                    await coinService.AddCoinAsync(coin.Id, cancellationToken);
                    return NoContent();
                },
                () => NotFound());
        }
    }
}
