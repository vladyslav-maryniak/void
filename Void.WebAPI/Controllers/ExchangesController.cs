using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Exchange>>> GetExchangesAsync(CancellationToken cancellationToken)
        {
            var exchanges = await exchangeService.GetExchangesAsync(cancellationToken);
            return Ok(mapper.Map<IEnumerable<ExchangeReadDto>>(exchanges));
        }

        [HttpGet("{id}", Name = "GetExchangeAsync")]
        public async Task<ActionResult<ExchangeReadDto>> GetExchangeAsync(string id, CancellationToken cancellationToken)
        {
            var exchange = await exchangeService.GetExchangeAsync(id, cancellationToken);
            if (exchange is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ExchangeReadDto>(exchange));
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeReadDto>> AddExchangeAsync(ExchangeAddDto exchangeAddDto, CancellationToken cancellationToken)
        {
            var exchange = mapper.Map<Exchange>(exchangeAddDto);

            try
            {
                await exchangeService.AddExchangeAsync(exchange, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            var exchangeReadDto = mapper.Map<ExchangeReadDto>(exchange);

            return CreatedAtRoute(nameof(GetExchangeAsync), new { Id = exchangeReadDto.Id }, exchangeReadDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveExchangeAsync(string id, CancellationToken cancellationToken)
        {
            await exchangeService.RemoveExchangeAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
