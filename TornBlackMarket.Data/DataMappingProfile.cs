﻿using AutoMapper;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Data.Models;

namespace TornBlackMarket.Data
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<ProfileDocumentDTO, ProfileDocumentBase>().ReverseMap();
            CreateMap<ProfileDocumentDTO, ProfileDocument>().ReverseMap();
            CreateMap<ItemDocumentDTO, ItemDocument>().ReverseMap();
            CreateMap<ExchangeDocumentDTO, ExchangeDocument>().ReverseMap();
        }
    }
}
