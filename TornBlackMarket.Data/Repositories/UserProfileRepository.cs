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
    public class UserProfileRepository : DataStoreRepository<UserProfileRepository>, IUserProfileRepository
    {
        public UserProfileRepository(SqlConnection _database, ILogger<UserProfileRepository> logger, IServiceProvider serviceProvider, IMapper mapper) 
            : base(_database,logger, serviceProvider, mapper)
        {
        }

        public async Task<UserProfileDocumentDTO?> CreateAsync(UserInfoDTO userInfoDto, string apiKey)
        {
            try
            {
                var document = new UserProfileDocument()
                {
                    Id = userInfoDto.Id,
                    Name = userInfoDto.Name,
                    ApiKey = apiKey
                };

                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(UserProfileDocument), JsonSerializer.Serialize(document));
                var ret = await Connection.InsertAsync<UserProfileDocument>(document);

                return await GetAsync(userInfoDto.Id);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to create {TableName} record: {Message}", nameof(UserProfileDocument), e.Message);
                return null;
            }
        }

        public async Task<UserProfileDocumentDTO?> GetAsync(string userId)
        {
            try
            {
                Logger.LogDebug("Fetching {TableName} for {UserId}", nameof(UserProfileDocument), userId);
                var profile = await Connection.GetAsync<UserProfileDocument>(userId);
                return Mapper.Map<UserProfileDocumentDTO>(profile);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to fetch {TableName} record: {Message}", nameof(UserProfileDocument), e.Message);
                return null;
            }
        }

        public async Task<UserInfoDTO?> GetUserAsync(string userId)
        {
            Logger.LogDebug("Fetching {TableName} for {UserId}", nameof(UserProfileDocument), userId);
            var profile = await Connection.GetAsync<UserProfileDocument>(userId);
            return Mapper.Map<UserInfoDTO>(profile);
        }

        public async Task<bool> UpdateAsync(UserProfileDocumentDTO profileDto)
        {
            try
            {
                var document = Mapper.Map<UserProfileDocument>(profileDto);
                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(UserProfileDocument), JsonSerializer.Serialize(document));
                var ret = await Connection.UpdateAsync<UserProfileDocument>(document);

                return ret;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to update {TableName} record: {Message}", nameof(UserProfileDocument), e.Message);
                return false;
            }
        }
    }
}
