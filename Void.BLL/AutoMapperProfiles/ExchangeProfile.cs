using AutoMapper;
using Void.DAL.Entities;
using Void.Shared.DTOs.Exchange;

namespace Void.BLL.AutoMapperProfiles
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
