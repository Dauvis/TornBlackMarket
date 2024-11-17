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
                Logger.LogDebug("Inserting {TableName} record:", nameof(UserProfileDocument));
                Logger.LogDebug("  Id = {Id}, ApiKey = {ApiKey}, Name = {Name}",
                    userInfoDto.Id, apiKey, userInfoDto.Name);

                var sql = "INSERT INTO UserProfileDocument (Id, ApiKey, Name) VALUES (@Id, @ApiKey, @Name);";
                int ret = await Connection.ExecuteAsync(sql, new { userInfoDto.Id, ApiKey = apiKey, userInfoDto.Name });

                if (ret != 0)
                {
                    return await GetAsync(userInfoDto.Id);
                }

                Logger.LogDebug("Failed to create {TableName} record: reason unknown", nameof(UserProfileDocument));
                return null;
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
                var sql = "SELECT * FROM UserProfileDocument WHERE Id = @Id;";
                var profile = await Connection.QuerySingleOrDefaultAsync<UserProfileDocument>(sql, new { Id = userId });

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
            Logger.LogDebug("Fetching {TableName} for {UserId} as a {TargetType}", nameof(UserProfileDocument), userId, nameof(UserInfoDTO));
            var profile = await GetAsync(userId);

            return Mapper.Map<UserInfoDTO>(profile);
        }

        public async Task<bool> UpdateAsync(UserProfileDocumentDTO profile)
        {
            try
            {
                Logger.LogDebug("Updating {TableName} record:", nameof(UserProfileDocument));
                Logger.LogDebug("  Id = {Id}, ApiKey = {ApiKey}, Name = {Name}",
                    profile.Id, profile.ApiKey, profile.Name);

                var sql = @"UPDATE UserProfileDocument 
                    SET Name = @Name, ApiKey = @ApiKey
                    WHERE Id = @Id";

                int ret = await Connection.ExecuteAsync(sql, new { profile.Id, profile.ApiKey, profile.Name });

                return ret > 0;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to update {TableName} record: {Message}", nameof(UserProfileDocument), e.Message);
                return false;
            }
        }
    }
}
