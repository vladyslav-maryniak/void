using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.DTOs.Ticker;

namespace Void.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TickersController : ControllerBase
    {
        private readonly ITickerService tickerService;
        private readonly IMapper mapper;

        public TickersController(ITickerService tickerService, IMapper mapper)
        {
            this.tickerService = tickerService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<TickerReadDto[]>> GetTickersAsync(CancellationToken cancellationToken)
        {
            var tickers = await tickerService.GetTickersAsync(cancellationToken);
            return Ok(mapper.Map<TickerReadDto[]>(tickers));
        }

        [HttpGet("coin/{coinId}")]
        public async Task<ActionResult<TickerReadDto[]>> GetTickersAsync(string coinId, CancellationToken cancellationToken)
        {
            var tickers = await tickerService.GetTickersAsync(coinId, cancellationToken);
            return Ok(mapper.Map<TickerReadDto[]>(tickers));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TickerReadDto>> GetTickerAsync(int id, CancellationToken cancellationToken)
        {
            var tickerOption = await tickerService.GetTickerAsync(id, cancellationToken);
            return tickerOption.Match<ActionResult<TickerReadDto>>(ticker =>
                Ok(mapper.Map<TickerReadDto>(ticker)), NotFound());
        }
    }
}
