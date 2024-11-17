using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;

namespace TornBlackMarket.Common.Interfaces
{
    public interface IUserProfileService
    {
        Task<AuthenticationResponseDTO> AuthenticateAsync(AuthenticationRequestDTO request);
        Task<UserInfoDTO?> GetAsync(string userId);
        Task<UserProfileDocumentDTO?> GetProfileAsync(string userId);
    }
}
