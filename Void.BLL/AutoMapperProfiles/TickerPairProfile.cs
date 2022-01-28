using AutoMapper;
using Void.BLL.Models;
using Void.Shared.DTOs.TickerPair;

namespace Void.BLL.AutoMapperProfiles
{
    public class TickerPairProfile : Profile
    {
        public TickerPairProfile()
        {
            CreateMap<TickerPair, TickerPairReadDto>();
        }
    }
}
