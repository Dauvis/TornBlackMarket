using System.Text.Json.Serialization;

namespace TornBlackMarket.Common.DTO.Domain
{
    public class TornItemDTO
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Effect { get; set; } = "";
        public string Requirement { get; set; } = "";
        public string Type { get; set; } = "";
        [JsonPropertyName("weapon_type")]
        public string WeaponType { get; set; } = "";
        [JsonPropertyName("buy_price")]
        public double BuyPrice { get; set; }
        [JsonPropertyName("sell_price")]
        public double SellPrice { get; set; }
        [JsonPropertyName("market_value")]
        public double MarketValue { get; set; }
        public int Circulation { get; set; }
        public string Image { get; set; } = "";
        public Dictionary<string, double> Coverage { get; set; } = [];
    }
}
