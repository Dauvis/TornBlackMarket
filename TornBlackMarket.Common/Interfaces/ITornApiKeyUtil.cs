using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface ITornApiKeyUtil
    {
        Task<(ProfileDocumentDTO?, bool)> ProfileDocumentForApiKeyAsync(string? apiKey);
    }
}
