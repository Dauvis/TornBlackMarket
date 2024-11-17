using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface ITornApiService
    {
        Task<TornPlayerDTO?> GetUserBasicAsync(string apiKey);
    }
}
