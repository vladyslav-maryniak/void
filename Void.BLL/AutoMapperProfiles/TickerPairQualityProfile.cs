using AutoMapper;
using Void.BLL.Models;
using Void.Shared.DTOs.TickerPairQuality;

namespace Void.BLL.AutoMapperProfiles
{
    public class TickerPairQualityProfile : Profile
    {
        public TickerPairQualityProfile()
        {
            CreateMap<TickerPairQuality, TickerPairQualityReadDto>();
        }
    }
}
