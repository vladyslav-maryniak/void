using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.Shared.DTOs.Ticker;

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
        public async Task<ActionResult<TickerReadDto[]>> GetTickersAsync()
        {
            var tickers = await tickerService.GetTickersAsync();
            return Ok(mapper.Map<TickerReadDto[]>(tickers));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TickerReadDto>> GetTickerAsync(int id)
        {
            var ticker = await tickerService.GetTickerAsync(id);
            if (ticker is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<TickerReadDto>(ticker));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveTickerAsync(int id)
        {
            try
            {
                await tickerService.RemoveTickerAsync(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
