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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TickerReadDto>> GetTickerAsync(int id, CancellationToken cancellationToken)
        {
            var ticker = await tickerService.GetTickerAsync(id, cancellationToken);
            if (ticker is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<TickerReadDto>(ticker));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveTickerAsync(int id, CancellationToken cancellationToken)
        {
            await tickerService.RemoveTickerAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
