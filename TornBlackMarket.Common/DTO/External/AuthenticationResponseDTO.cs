using TornBlackMarket.Common.Enums;

namespace TornBlackMarket.Common.DTO.External
{
    public class AuthenticationResponseDTO
    {
        public string WebToken { get; set; } = "";
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public ErrorCodesType ErrorCode { get; set; } = ErrorCodesType.None;
        public string ErrorMessage { get; set; } = "";
    }
}
