using AutoMapper;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Data.Models;

namespace TornBlackMarket.Data
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<ProfileDocumentDTO, ProfileDocument>().ReverseMap();
            CreateMap<ItemDocumentDTO, ItemDocument>().ReverseMap();
        }
    }
}
