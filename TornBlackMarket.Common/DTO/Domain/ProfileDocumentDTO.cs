using TornBlackMarket.Common.Enums;

namespace TornBlackMarket.Common.DTO.Domain
{
    public class ProfileDocumentDTO
    {
        public string Id { get; set; } = "";
        public string ApiKey { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Web { get; set; } = "";
        public string Discord { get; set; } = "";
        public DateTimeOffset TokenInvalidDateTime { get; set; } = DateTimeOffset.MinValue;
    }
}
