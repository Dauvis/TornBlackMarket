using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;

namespace TornBlackMarket.Common.Interfaces
{
    public interface IProfileService
    {
        Task<AuthenticationResponseDTO> AuthenticateAsync(AuthenticationRequestDTO request);
        Task<ProfileDocumentDTO?> GetAsync(string profileId);
        Task<bool> InvalidateTokensAsync(string profileId);
        Task<bool> UpdateAsync(string profileId, ProfileDocumentDTO profileDto);
    }
}
