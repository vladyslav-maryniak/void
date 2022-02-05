using AutoMapper;
using Void.DAL.Entities;
using Void.WebAPI.DTOs.Ticker;

namespace Void.BLL.AutoMapperProfiles
{
    public class TickerProfile : Profile
    {
        public TickerProfile()
        {
            CreateMap<Ticker, TickerReadDto>();
        }
    }
}
