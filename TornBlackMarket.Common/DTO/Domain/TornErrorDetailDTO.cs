using System.Text.Json.Serialization;

namespace TornBlackMarket.Common.DTO.Domain
{
    public class TornErrorDetailDTO
    {
        public int Code { get; set; } = 0;
        [JsonPropertyName("error")]
        public string Message { get; set; } = "";
    }
}
