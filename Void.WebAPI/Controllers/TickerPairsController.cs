using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.DTOs.TickerPair;

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

        [HttpGet("{coinId:regex(^([[a-z0-9]]*)(-[[a-z0-9]]+)*$):length(1,55)}")]
        public async Task<ActionResult<TickerPairReadDto>> GetTickerPairAsync(string coinId, bool defaultFilters, CancellationToken cancellationToken)
        {
            var tickerPairOption = await tickerPairService.GetTickerPairAsync(coinId, defaultFilters, cancellationToken);
            return tickerPairOption.Match<ActionResult<TickerPairReadDto>>(tickerPair =>
                Ok(mapper.Map<TickerPairReadDto>(tickerPair)), NotFound());
        }
    }
}
