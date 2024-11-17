using AutoMapper;
using System.Reflection;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;

namespace TornBlackMarket.Server
{
    public class ServerMappingProfile : Profile
    {
        public ServerMappingProfile()
        {
            CreateMap<UserProfileDocumentDTO, TbmProfileDTO>();
            CreateMap<UserInfoDTO, TbmUserDTO>();
        }
    }
}
