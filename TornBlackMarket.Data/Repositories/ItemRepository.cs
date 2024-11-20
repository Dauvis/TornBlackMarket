using AutoMapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Abstraction;
using TornBlackMarket.Data.Attributes;
using TornBlackMarket.Data.Models;

namespace TornBlackMarket.Data.Repositories
{
    [DataStoreRepository("Main")]
    public class ItemRepository : DataStoreRepository<ItemRepository>, IItemRepository
    {
        public ItemRepository(SqlConnection database, ILogger<ItemRepository> logger, IServiceProvider serviceProvider, IMapper mapper) : 
            base(database, logger, serviceProvider, mapper)
        {
        }

        public async Task<ItemDocumentDTO?> CreateAsync(ItemDocumentDTO itemDto)
        {
            try
            {
                var document = Mapper.Map<ItemDocument>(itemDto);
                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(ItemDocument), JsonSerializer.Serialize(document));
                var ret = await Connection.InsertAsync<ItemDocument>(document);

                return await GetAsync(itemDto.Id);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to create {TableName} record: {Message}", nameof(ItemDocument), e.Message);
                return null;
            }

        }

        public async Task<ItemDocumentDTO?> GetAsync(string itemId)
        {
            try
            {
                Logger.LogDebug("Fetching {TableName} for {ItemId}", nameof(ItemDocument), itemId);
                var item = await Connection.GetAsync<ItemDocument>(itemId);
                return Mapper.Map<ItemDocumentDTO>(item);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to fetch {TableName} record: {Message}", nameof(ItemDocument), e.Message);
                return null;
            }
        }

        public async Task<List<ItemDocumentDTO>> FetchAsync()
        {
            try
            {
                Logger.LogDebug("Fetching all {TableName} records", nameof(ItemDocument));
                var items = await Connection.GetAllAsync<ItemDocument>();
                return Mapper.Map<List<ItemDocumentDTO>>(items);
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to fetch {TableName} record: {Message}", nameof(ItemDocument), e.Message);
                return [];
            }
        }

        public async Task<bool> UpdateAsync(ItemDocumentDTO itemDto)
        {
            try
            {
                var document = Mapper.Map<ItemDocument>(itemDto);
                Logger.LogDebug("Inserting {TableName} record: {SerializedData}", nameof(ItemDocument), JsonSerializer.Serialize(document));
                var ret = await Connection.UpdateAsync<ItemDocument>(document);

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
