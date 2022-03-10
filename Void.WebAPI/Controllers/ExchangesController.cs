using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.WebAPI.Contracts;
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
        public async Task<ActionResult<ExchangeReadDto[]>> GetExchangesAsync(CancellationToken cancellationToken)
        {
            var exchanges = await exchangeService.GetExchangesAsync(cancellationToken);
            return Ok(mapper.Map<ExchangeReadDto[]>(exchanges));
        }

        [HttpGet("{id:regex(^([[a-z0-9]]*)(-[[a-z0-9]]+)*$):length(1,35)}", Name = "GetExchangeAsync")]
        public async Task<ActionResult<ExchangeReadDto>> GetExchangeAsync(string id, CancellationToken cancellationToken)
        {
            var exchangeOption = await exchangeService.GetExchangeAsync(id, cancellationToken);
            return exchangeOption.Match<ActionResult<ExchangeReadDto>>(exchange =>
                Ok(mapper.Map<ExchangeReadDto>(exchange)), NotFound());
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeReadDto>> AddExchangeAsync(
            ExchangeAddDto exchangeDto, CancellationToken cancellationToken)
        {
            var exchangeOption = await exchangeService.AddExchangeAsync(exchangeDto.Id, cancellationToken);
            return exchangeOption.Match<ActionResult<ExchangeReadDto>>(
                exchange =>
                {
                    var exchangeDto = mapper.Map<ExchangeReadDto>(exchange);
                    return CreatedAtRoute(nameof(GetExchangeAsync), new { exchange.Id }, exchangeDto);
                },
                () =>
                {
                    ValidationErrorResponse error = new();
                    ValidationErrorModel model = new()
                    {
                        PropertyName = nameof(exchangeDto.Id),
                        Message = "Exchange with provided 'id' already exists"
                    };
                    error.Errors.Add(model);

                    return BadRequest(error);
                });
        }

        [HttpDelete("{id:regex(^([[a-z0-9]]*)(-[[a-z0-9]]+)*$):length(1,35)}")]
        public async Task<ActionResult> RemoveExchangeAsync(string id, CancellationToken cancellationToken)
        {
            var exchangeOption = await exchangeService.RemoveExchangeAsync(id, cancellationToken);
            return exchangeOption.Match<ActionResult>(_ => NoContent(), NotFound());
        }
    }
}
