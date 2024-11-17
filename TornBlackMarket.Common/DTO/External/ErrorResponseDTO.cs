using TornBlackMarket.Common.Enums;

namespace TornBlackMarket.Common.DTO.External
{
    public class ErrorResponseDTO
    {
        public ErrorCodesType ErrorCode { get; set; } = ErrorCodesType.None;
        public string ErrorMessage { get; set; } = "";
    }
}
