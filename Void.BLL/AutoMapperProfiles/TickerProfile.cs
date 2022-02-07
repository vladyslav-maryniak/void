using AutoMapper;
using Void.BLL.DTOs.Ticker;
using Void.DAL.Entities;

namespace Void.BLL.AutoMapperProfiles
{
    public class TickerProfile : Profile
    {
        public TickerProfile()
        {
            CreateMap<CoinGeckoTickerDto, Ticker>()
                .ForMember(dest => dest.ExchangeId, opt => opt.MapFrom(src => src.Market.Identifier));
        }
    }
}
