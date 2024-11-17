using AutoMapper;
using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Data
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<UserProfileDocumentDTO, Models.UserProfileDocument>().ReverseMap();
            CreateMap<UserProfileDocumentDTO, UserInfoDTO>();
        }
    }
}
