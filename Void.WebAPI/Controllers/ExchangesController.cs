using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.WebAPI.DTOs.Exchange;

namespace Void.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExchangesController : ControllerBase
    {
        private readonly IExchangeService exchangeService;
        private readonly IMapper mapper;

        public ExchangesController(IExchangeService exchangeService, IMapper mapper)
        {
            this.exchangeService = exchangeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Exchange[]>> GetExchangesAsync(CancellationToken cancellationToken)
        {
            var exchanges = await exchangeService.GetExchangesAsync(cancellationToken);
            return Ok(mapper.Map<ExchangeReadDto[]>(exchanges));
        }

        [HttpGet("{id}", Name = "GetExchangeAsync")]
        public async Task<ActionResult<ExchangeReadDto>> GetExchangeAsync(string id, CancellationToken cancellationToken)
        {
            var exchangeOption = await exchangeService.GetExchangeAsync(id, cancellationToken);
            return exchangeOption.Match<ActionResult<ExchangeReadDto>>(exchange =>
                Ok(mapper.Map<ExchangeReadDto>(exchange)), NotFound());
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeReadDto>> AddExchangeAsync(
            ExchangeAddDto exchangeAddDto, CancellationToken cancellationToken)
        {
            var exchangeOption = await exchangeService.AddExchangeAsync(exchangeAddDto.Id, cancellationToken);
            return exchangeOption.Match<ActionResult<ExchangeReadDto>>(
                exchange => CreatedAtRoute(nameof(GetExchangeAsync), new { exchange.Id }, mapper.Map<ExchangeReadDto>(exchange)),
                BadRequest());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveExchangeAsync(string id, CancellationToken cancellationToken)
        {
            var exchangeOption = await exchangeService.RemoveExchangeAsync(id, cancellationToken);
            return exchangeOption.Match<ActionResult>(_ => NoContent(), NotFound());
        }
    }
}
