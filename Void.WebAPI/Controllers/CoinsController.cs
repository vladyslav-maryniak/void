using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
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
            CoinAddDto coinAddDto, CancellationToken cancellationToken)
        {
            var coinOption = await coinService.AddCoinAsync(coinAddDto.Id, cancellationToken);
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
                        PropertyName = nameof(coinAddDto.Id),
                        Message = "Coin with provided 'id' already exists"
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
    }
}
