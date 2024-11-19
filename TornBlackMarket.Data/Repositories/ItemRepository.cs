using AutoMapper;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Abstraction;
using TornBlackMarket.Data.Attributes;
using TornBlackMarket.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                Logger.LogDebug("Inserting {TableName} record:", nameof(ItemDocument));
                Logger.LogDebug("  Id = {Id}, Name = {Name}, Description = {Description}",
                    itemDto.Id, itemDto.Name, itemDto.Description);
                Logger.LogDebug("  Effect = {Effect}, Requirement = {Requirment}, Type = {Type}",
                    itemDto.Effect, itemDto.Requirement, itemDto.Type);
                Logger.LogDebug("  WeaponType = {WeaponType}, BuyPrice = {BuyPrice}, SellPrice = {SellPrice}",
                    itemDto.WeaponType, itemDto.BuyPrice, itemDto.SellPrice);
                Logger.LogDebug("  MarketValue = {MarketValue}, Circulation = {Circulation}, ImageUrl = {ImageUrl}",
                    itemDto.MarketValue, itemDto.Circulation, itemDto.ImageUrl);

                //var sql = @"INSERT INTO Items 
                //    (Id, Name, Description, Effect, Requirement,
                //    Type, WeaponType, BuyPrice, SellPrice, MarketValue,
                //    Circulation, ImageUrl) 
                //    VALUES 
                //    (@Id, @Name, @Description, @Effect, @Requirement,
                //    @Type, @WeaponType, @BuyPrice, @SellPrice, @MarketValue,
                //    @Circulation, @ImageUrl);";

                //int ret = await Connection.ExecuteAsync(sql, 
                //    new { itemDto.Id, itemDto.Name, itemDto.Description, itemDto.Effect, itemDto.Requirement,
                //    itemDto.Type, itemDto.WeaponType, itemDto.BuyPrice, itemDto.SellPrice, itemDto.MarketValue,
                //    itemDto.Circulation, itemDto.ImageUrl});

                var document = Mapper.Map<ItemDocument>(itemDto);
                var ret = await Connection.InsertAsync<ItemDocument>(document);

                if (ret != 0)
                {
                    return await GetAsync(itemDto.Id);
                }

                Logger.LogDebug("Failed to create {TableName} record: reason unknown", nameof(ItemDocument));
                return null;
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

                //var sql = "SELECT * FROM Items WHERE Id = @Id;";
                //var item = await Connection.QuerySingleOrDefaultAsync<ItemDocument>(sql, new { Id = itemId });

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

                //var sql = "SELECT * FROM Items";
                //var items = await Connection.QueryAsync<ItemDocument>(sql);

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
                Logger.LogDebug("Updating {TableName} record:", nameof(ItemDocument));
                Logger.LogDebug("  Id = {Id}, Name = {Name}, Description = {Description}",
                    itemDto.Id, itemDto.Name, itemDto.Description);
                Logger.LogDebug("  Effect = {Effect}, Requirement = {Requirment}, Type = {Type}",
                    itemDto.Effect, itemDto.Requirement, itemDto.Type);
                Logger.LogDebug("  WeaponType = {WeaponType}, BuyPrice = {BuyPrice}, SellPrice = {SellPrice}",
                    itemDto.WeaponType, itemDto.BuyPrice, itemDto.SellPrice);
                Logger.LogDebug("  MarketValue = {MarketValue}, Circulation = {Circulation}, ImageUrl = {ImageUrl}",
                    itemDto.MarketValue, itemDto.Circulation, itemDto.ImageUrl);

                //var sql = @"UPDATE UserProfileDocument 
                //    SET Name = @Name, Description = @Description, Effect = @Effect, Requirement = @Requirement,
                //    Type = @Type, WeaponType = @WeaponType, BuyPrice = @BuyPrice, SellPrice = @SellPrice,
                //    MarketValue = @MarketValue, Circulation = @Circulation, ImageUrl = @ImageUrl
                //    WHERE Id = @Id"
                //;
                //int ret = await Connection.ExecuteAsync(sql, new { 
                //    itemDto.Id, itemDto.Name, itemDto.Description, itemDto.Effect, itemDto.Requirement,
                //    itemDto.Type, itemDto.WeaponType, itemDto.BuyPrice, itemDto.SellPrice, itemDto.MarketValue,
                //    itemDto.Circulation, itemDto.ImageUrl});

                var document = Mapper.Map<ItemDocument>(itemDto);
                var ret = await Connection.UpdateAsync<ItemDocument>(document);

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
