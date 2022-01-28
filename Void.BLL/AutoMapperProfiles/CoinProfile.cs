using AutoMapper;
using Void.DAL.Entities;
using Void.Shared.DTOs.Coin;

namespace Void.BLL.AutoMapperProfiles
{
    public class CoinProfile : Profile
    {
        public CoinProfile()
        {
            CreateMap<CoinAddDto, Coin>();
            CreateMap<Coin, CoinReadDto>();
            CreateMap<CoinReadDto, Coin>();
        }
    }
}
