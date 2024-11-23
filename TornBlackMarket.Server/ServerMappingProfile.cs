using AutoMapper;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;

namespace TornBlackMarket.Server
{
    public class ServerMappingProfile : Profile
    {
        public ServerMappingProfile()
        {
            CreateMap<ProfileDocumentDTO, TbmProfileDTO>();
            CreateMap<ExchangeDocumentDTO, TbmExchangeDTO>();
        }
    }
}
