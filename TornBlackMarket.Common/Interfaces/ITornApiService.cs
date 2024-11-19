using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface ITornApiService
    {
        Task<TornItemsDTO?> GetItemsAsync(string apiKey);
        Task<TornPlayerDTO?> GetUserBasicAsync(string apiKey);
    }
}
