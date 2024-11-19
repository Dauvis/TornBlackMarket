using Microsoft.Extensions.Logging;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Periodic.Enums;
using TornBlackMarket.Periodic.Interfaces;

namespace TornBlackMarket.Periodic.Services
{
    public class ItemLoadJobService : JobServiceBase, IItemLoadJobService
    {
        private readonly IItemService _itemService;
        private readonly ITornApiService _tornApi;
        private readonly ILogger<ItemLoadJobService> _logger;

        public ItemLoadJobService(IItemService itemService, ITornApiService tornApi, ILogger<ItemLoadJobService> logger)
        {
            _itemService = itemService;
            _tornApi = tornApi;
            _logger = logger;
        }

        public override async Task ExecuteAsync(JobSettings settings)
        {
            var dbItems = await _itemService.FetchAsync();
            var apiItems = await _tornApi.GetItemsAsync(settings.ApiKey);
            
            if (apiItems is null || apiItems.ItemsDictionary.Count == 0)
            {
                _logger.LogError("Failed to load item data from Torn API (Job: {JobId}", JobIdType.ItemLoad);
                return;
            }

            await ProcessItemsAsync(dbItems, apiItems);
        }

        private async Task ProcessItemsAsync(List<ItemDocumentDTO> dbItems, TornItemsDTO apiItems)
        {
            HashSet<string> updatedItems = [];

            foreach (var dbItem in dbItems)
            {
                if (apiItems.ItemsDictionary.TryGetValue(dbItem.Id, out TornItemDTO? apiItem))
                {
                    dbItem.MarketValue = apiItem.MarketValue;
                    dbItem.Circulation = apiItem.Circulation;
                    await _itemService.UpdateAsync(dbItem);
                    updatedItems.Add(dbItem.Id);
                }
            }

            foreach (var apiItem in apiItems.ItemsDictionary)
            {
                var id = apiItem.Key;
                var tornItem = apiItem.Value;

                if (!updatedItems.Contains(id))
                {
                    var itemDto = new ItemDocumentDTO()
                    {
                        Id = id,
                        Name = tornItem.Name,
                        Description = tornItem.Description,
                        Effect = tornItem.Effect,
                        Requirement = tornItem.Requirement,
                        Type = tornItem.Type,
                        WeaponType = tornItem.WeaponType,
                        BuyPrice = tornItem.BuyPrice,
                        SellPrice = tornItem.SellPrice,
                        MarketValue = tornItem.MarketValue,
                        Circulation = tornItem.Circulation,
                        ImageUrl = tornItem.Image
                    };

                    await _itemService.CreateAsync(itemDto);
                }
            }
        }
    }
}
