using AutoMapper;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Data.Models;

namespace TornBlackMarket.Data
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<UserProfileDocumentDTO, UserProfileDocument>().ReverseMap();
            CreateMap<UserProfileDocumentDTO, UserInfoDTO>();
            CreateMap<UserProfileDocument, UserInfoDTO>();
            CreateMap<ItemDocumentDTO, Models.ItemDocument>().ReverseMap();
        }
    }
}
