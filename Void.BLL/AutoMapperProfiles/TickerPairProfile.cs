using AutoMapper;
using Void.BLL.DTOs.Ticker;
using Void.BLL.Models;

namespace Void.BLL.AutoMapperProfiles
{
    public class TickerPairProfile : Profile
    {
        public TickerPairProfile()
        {
            CreateMap<TickerPair, TickerPairNotificationReadDto>()
                .ForMember(dest => dest.Coin, opt => opt.MapFrom(src => src.Supply.Coin))
                .ForMember(dest => dest.ProfitPercentage, opt => opt.MapFrom(src => src.Quality.ProfitPercentage));
        }
    }
}
