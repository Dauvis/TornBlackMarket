using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface IProfileRepository
    {
        Task<ProfileDocumentDTO?> CreateAsync(ProfileDocumentDTO profileDto, string apiKey);
        Task<ProfileDocumentDTO?> GetAsync(string profileId);
        Task<bool> UpdateAsync(ProfileDocumentDTO profile, string newApiKey = "");
    }
}
