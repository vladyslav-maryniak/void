using AutoMapper;
using Void.BLL.DTOs.Coin;
using Void.DAL.Entities;

namespace Void.BLL.AutoMapperProfiles
{
    public class CoinProfile : Profile
    {
        public CoinProfile()
        {
            CreateMap<CoinGeckoCoinReadDto, Coin>();
            CreateMap<Coin, CoinNotificationReadDto>();
        }
    }
}
