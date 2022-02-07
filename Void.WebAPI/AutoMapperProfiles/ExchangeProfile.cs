using AutoMapper;
using Void.DAL.Entities;
using Void.WebAPI.DTOs.Exchange;

namespace Void.WebAPI.AutoMapperProfiles
{
    public class ExchangeProfile : Profile
    {
        public ExchangeProfile()
        {
            CreateMap<ExchangeAddDto, Exchange>();
            CreateMap<Exchange, ExchangeReadDto>();
            CreateMap<ExchangeReadDto, Exchange>();
        }
    }
}
