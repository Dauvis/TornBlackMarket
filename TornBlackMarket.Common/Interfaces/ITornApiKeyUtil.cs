using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface ITornApiKeyUtil
    {
        Task<UserProfileDocumentDTO?> ProfileDocumentForApiKeyAsync(string? apiKey);
    }
}
