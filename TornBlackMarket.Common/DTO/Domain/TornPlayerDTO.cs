using System.Text.Json.Serialization;

namespace TornBlackMarket.Common.DTO.Domain
{
    public class TornPlayerDTO
    {
        public int Level { get; set; }
        public string Gender { get; set; } = "";
        [JsonPropertyName("player_id")]
        public int PlayerId { get; set; }
        public string Name { get; set; } = "";
        public TornPlayerStatusDTO Status { get; set; } = new();
        public TornErrorDTO? Error { get; set; }

        public static TornPlayerDTO CreateError(TornErrorDTO? error)
        {
            return new()
            {
                Error = error
            };
        }
    }
}
