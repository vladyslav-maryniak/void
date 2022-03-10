using AutoMapper;
using Void.DAL.Entities;
using Void.WebAPI.DTOs.Coin;

namespace Void.WebAPI.AutoMapperProfiles
{
    public class CoinProfile : Profile
    {
        public CoinProfile()
        {
            CreateMap<CoinAddDto, Coin>();
            CreateMap<Coin, CoinReadDto>();

            CreateMap<BlacklistedCoinAddDto, BlacklistedCoin>();
            CreateMap<BlacklistedCoin, BlacklistedCoinReadDto>();
        }
    }
}
