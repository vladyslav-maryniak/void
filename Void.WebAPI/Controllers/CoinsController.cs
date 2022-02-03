using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Void.BLL.Services.Abstractions;
using Void.DAL.Entities;
using Void.Shared.DTOs.Coin;

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
        public async Task<ActionResult<IEnumerable<Coin>>> GetCoinsAsync(CancellationToken cancellationToken)
        {
            var coins = await coinService.GetCoinsAsync(cancellationToken);
            return Ok(mapper.Map<IEnumerable<CoinReadDto>>(coins));
        }

        [HttpGet("{id}", Name = "GetCoinAsync")]
        public async Task<ActionResult<CoinReadDto>> GetCoinAsync(string id, CancellationToken cancellationToken)
        {
            var coin = await coinService.GetCoinAsync(id, cancellationToken);
            if (coin is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CoinReadDto>(coin));
        }

        [HttpPost]
        public async Task<ActionResult<CoinReadDto>> AddCoinAsync(CoinAddDto coinAddDto, CancellationToken cancellationToken)
        {
            var coin = mapper.Map<Coin>(coinAddDto);

            try
            {
                await coinService.AddCoinAsync(coin, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            var coinReadDto = mapper.Map<CoinReadDto>(coin);

            return CreatedAtRoute(nameof(GetCoinAsync), new { Id = coinReadDto.Id }, coinReadDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCoinAsync(string id, CancellationToken cancellationToken)
        {
            await coinService.RemoveCoinAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
