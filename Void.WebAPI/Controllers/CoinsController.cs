using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Coin>>> GetCoinsAsync()
        {
            var coins = await coinService.GetCoinsAsync();
            return Ok(mapper.Map<IEnumerable<CoinReadDto>>(coins));
        }

        [HttpGet("{id}", Name = "GetCoinAsync")]
        public async Task<ActionResult<CoinReadDto>> GetCoinAsync(string id)
        {
            var coin = await coinService.GetCoinAsync(id);
            if (coin is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CoinReadDto>(coin));
        }

        [HttpPost]
        public async Task<ActionResult<CoinReadDto>> AddCoinAsync(CoinAddDto coinAddDto)
        {
            var coin = mapper.Map<Coin>(coinAddDto);

            try
            {
                await coinService.AddCoinAsync(coin);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            var coinReadDto = mapper.Map<CoinReadDto>(coin);

            return CreatedAtRoute(nameof(GetCoinAsync), new { Id = coinReadDto.Id }, coinReadDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCoinAsync(string id)
        {
            try
            {
                await coinService.RemoveCoinAsync(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
