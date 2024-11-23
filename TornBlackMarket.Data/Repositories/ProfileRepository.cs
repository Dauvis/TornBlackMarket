using AutoMapper;
using Microsoft.Extensions.Logging;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Abstraction;
using TornBlackMarket.Data.Attributes;
using TornBlackMarket.Data.Models;
using Microsoft.Data.SqlClient;
using TornBlackMarket.Common.DTO.Domain;
using System.Text.Json;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TornBlackMarket.Data.Repositories
{
    [DataStoreRepository("Main")]
    public class ProfileRepository : DataStoreRepository<ProfileRepository>, IProfileRepository
    {
        private readonly IEncryptionUtil _encryptionUtil;

        public ProfileRepository(SqlConnection _database, ILogger<ProfileRepository> logger, IServiceProvider serviceProvider, 
            IMapper mapper, IConfiguration configuration) 
            : base(_database,logger, serviceProvider, mapper, configuration)
        {
            _encryptionUtil = serviceProvider.GetRequiredService<IEncryptionUtil>();
        }

        public async Task<ProfileDocumentDTO?> CreateAsync(ProfileDocumentDTO profileDto, string apiKey)
        {
            try
            {
                byte[] vector = _encryptionUtil.GenerateVector(16);
                string encryptedKey = _encryptionUtil.Encrypt(apiKey, vector);

                var document = new ProfileDocument()
                {
                    Id = profileDto.Id,
                    Name = profileDto.Name,
                    ApiKey = encryptedKey,
                    ApiKeyVI = vector
                };

                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(ProfileDocumentBase), JsonSerializer.Serialize(document));
                var ret = await Connection.InsertAsync<ProfileDocument>(document);

                return await GetAsync(profileDto.Id);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to create {TableName} record: {Message}", nameof(ProfileDocumentBase), e.Message);
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

        public async Task<bool> UpdateAsync(ProfileDocumentDTO profileDto, string newApiKey = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(newApiKey))
                {
                    byte[] vector = _encryptionUtil.GenerateVector(16);
                    string encryptedKey = _encryptionUtil.Encrypt(newApiKey, vector);

                    profileDto.ApiKey = encryptedKey;
                    profileDto.ApiKeyVI = vector;

                    var document = Mapper.Map<ProfileDocument>(profileDto);
                    Logger.LogDebug("Updating {TableName} record: {SerializedData}", nameof(ProfileDocument), JsonSerializer.Serialize(document));
                    var retBase = await Connection.UpdateAsync<ProfileDocument>(document);

                    return retBase;
                }

                var documentBase = Mapper.Map<ProfileDocumentBase>(profileDto);
                Logger.LogDebug("Updating {TableName} record: {SerializedData}", nameof(ProfileDocument), JsonSerializer.Serialize(documentBase));
                var ret = await Connection.UpdateAsync<ProfileDocumentBase>(documentBase);

                return ret;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to update {TableName} record: {Message}", nameof(ProfileDocumentBase), e.Message);
                return false;
            }
        }
    }
}
