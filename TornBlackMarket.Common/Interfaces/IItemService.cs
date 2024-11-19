using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface IItemService
    {
        Task<ItemDocumentDTO?> CreateAsync(ItemDocumentDTO item);
        Task<List<ItemDocumentDTO>> FetchAsync();
        Task<ItemDocumentDTO?> GetAsync(string itemId);
        Task<bool> UpdateAsync(ItemDocumentDTO itemDto);
    }
}
