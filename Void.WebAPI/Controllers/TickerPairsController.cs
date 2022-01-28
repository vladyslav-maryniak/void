using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.Shared.DTOs.TickerPair;

namespace Void.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TickerPairsController : ControllerBase
    {
        private readonly ITickerPairService tickerPairService;
        private readonly IMapper mapper;

        public TickerPairsController(ITickerPairService tickerPairService, IMapper mapper)
        {
            this.tickerPairService = tickerPairService;
            this.mapper = mapper;
        }

        [HttpGet("{coinId}")]
        public async Task<ActionResult<TickerPairReadDto>> GetTickerPairAsync(string coinId, bool defaultFilters = false)
        {
            var tickerPair = await tickerPairService.GetTickerPairAsync(coinId, defaultFilters);
            return Ok(mapper.Map<TickerPairReadDto>(tickerPair));
        }
    }
}
