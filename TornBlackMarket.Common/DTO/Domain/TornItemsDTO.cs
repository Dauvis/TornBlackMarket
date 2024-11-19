using System.Text.Json.Serialization;

namespace TornBlackMarket.Common.DTO.Domain
{
    public class TornItemsDTO
    {
        [JsonPropertyName("items")]
        public Dictionary<string, TornItemDTO> ItemsDictionary { get; set; } = [];
    }
}
