using AutoMapper;
using Void.DAL.Entities;
using Void.Shared.DTOs.Ticker;

namespace Void.BLL.AutoMapperProfiles
{
    public class TickerProfile : Profile
    {
        public TickerProfile()
        {
            CreateMap<CoinGeckoTickerDto, Ticker>()
                .ForMember(dest => dest.ExchangeId, opt => opt.MapFrom(src => src.Market.Identifier));
            CreateMap<Ticker, TickerReadDto>();
        }
    }
}
