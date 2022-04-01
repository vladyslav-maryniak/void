using AutoMapper;
using Void.BLL.DTOs.Exchange;
using Void.DAL.Entities;

namespace Void.BLL.AutoMapperProfiles
{
    public class ExchangeProfile : Profile
    {
        public ExchangeProfile()
        {
            CreateMap<CoinGeckoExchangeReadDto, Exchange>();
            CreateMap<Exchange, ExchangeNotificationReadDto>();
        }
    }
}
