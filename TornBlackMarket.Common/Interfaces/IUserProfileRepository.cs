using TornBlackMarket.Common.DTO.Domain;

namespace TornBlackMarket.Common.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfileDocumentDTO?> CreateAsync(UserInfoDTO userInfoDto, string apiKey);
        Task<UserProfileDocumentDTO?> GetAsync(string userId);
        Task<UserInfoDTO?> GetUserAsync(string userId);
        Task<bool> UpdateAsync(UserProfileDocumentDTO profile);
    }
}
