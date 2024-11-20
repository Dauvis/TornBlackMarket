using AutoMapper;
using Microsoft.Extensions.Logging;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Abstraction;
using TornBlackMarket.Data.Attributes;
using TornBlackMarket.Data.Models;
using System.Net;
using Microsoft.Data.SqlClient;
using TornBlackMarket.Common.DTO.Domain;
using Dapper;
using static Dapper.SqlMapper;
using System.Text.Json;
using Dapper.Contrib.Extensions;

namespace TornBlackMarket.Data.Repositories
{
    [DataStoreRepository("Main")]
    public class UserProfileRepository : DataStoreRepository<UserProfileRepository>, IProfileRepository
    {
        public UserProfileRepository(SqlConnection _database, ILogger<UserProfileRepository> logger, IServiceProvider serviceProvider, IMapper mapper) 
            : base(_database,logger, serviceProvider, mapper)
        {
        }

        public async Task<ProfileDocumentDTO?> CreateAsync(ProfileDocumentDTO profileDto, string apiKey)
        {
            try
            {
                var document = new ProfileDocument()
                {
                    Id = profileDto.Id,
                    Name = profileDto.Name,
                    ApiKey = apiKey,                    
                };

                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(ProfileDocument), JsonSerializer.Serialize(document));
                var ret = await Connection.InsertAsync<ProfileDocument>(document);

                return await GetAsync(profileDto.Id);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to create {TableName} record: {Message}", nameof(ProfileDocument), e.Message);
                return null;
            }
        }

        public async Task<ProfileDocumentDTO?> GetAsync(string profileId)
        {
            try
            {
                Logger.LogDebug("Fetching {TableName} for {ProfileId}", nameof(ProfileDocument), profileId);
                var profile = await Connection.GetAsync<ProfileDocument>(profileId);
                return Mapper.Map<ProfileDocumentDTO>(profile);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to fetch {TableName} record: {Message}", nameof(ProfileDocument), e.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(ProfileDocumentDTO profileDto)
        {
            try
            {
                var document = Mapper.Map<ProfileDocument>(profileDto);
                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(ProfileDocument), JsonSerializer.Serialize(document));
                var ret = await Connection.UpdateAsync<ProfileDocument>(document);

                return ret;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to update {TableName} record: {Message}", nameof(ProfileDocument), e.Message);
                return false;
            }
        }
    }
}
