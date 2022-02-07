using AutoMapper;
using Void.BLL.Models;
using Void.WebAPI.DTOs.TickerPairQuality;

namespace Void.WebAPI.AutoMapperProfiles
{
    public class TickerPairQualityProfile : Profile
    {
        public TickerPairQualityProfile()
        {
            CreateMap<TickerPairQuality, TickerPairQualityReadDto>();
        }
    }
}
