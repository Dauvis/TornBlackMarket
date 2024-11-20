using TornBlackMarket.Common.Enums;

namespace TornBlackMarket.Common.DTO.External
{
    public class AuthenticationResponseDTO
    {
        public string WebToken { get; set; } = "";
        public string PlayerId { get; set; } = "";
        public string PlayerName { get; set; } = "";
        public ErrorCodesType ErrorCode { get; set; } = ErrorCodesType.None;
        public string ErrorMessage { get; set; } = "";
    }
}
