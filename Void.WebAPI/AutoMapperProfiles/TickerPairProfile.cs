using AutoMapper;
using Void.BLL.Models;
using Void.WebAPI.DTOs.TickerPair;

namespace Void.WebAPI.AutoMapperProfiles
{
    public class TickerPairProfile : Profile
    {
        public TickerPairProfile()
        {
            CreateMap<TickerPair, TickerPairReadDto>();
        }
    }
}
