using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.Interfaces;

namespace TornBlackMarket.Logic.Services
{
    public class ItemService : IItemService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private IItemRepository? _itemRepository;

        public ItemService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        protected IItemRepository GetItemRepository()
        {
            _itemRepository ??= _repositoryFactory.Create<IItemRepository>()
                    ?? throw new InvalidOperationException($"Failed to instantiate repository: {nameof(IItemRepository)}");

            return _itemRepository;
        }

        public async Task<ItemDocumentDTO?> CreateAsync(ItemDocumentDTO item)
        {
            var repository = GetItemRepository();
            return await repository.CreateAsync(item);
        }

        public async Task<ItemDocumentDTO?> GetAsync(string itemId)
        {
            var repository = GetItemRepository();
            return await repository.GetAsync(itemId);
        }

        public async Task<List<ItemDocumentDTO>> FetchAsync()
        {
            var repository = GetItemRepository();
            return await repository.FetchAsync();
        }

        public async Task<bool> UpdateAsync(ItemDocumentDTO itemDto)
        {
            var repository = GetItemRepository();
            return await repository.UpdateAsync(itemDto);
        }
    }
}
