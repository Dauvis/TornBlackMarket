using Dapper.Contrib.Extensions;

namespace TornBlackMarket.Data.Models
{
    [Table("Items")]
    public class ItemDocument
    {
        [ExplicitKey]
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Effect { get; set; } = "";
        public string Requirement { get; set; } = "";
        public string Type { get; set; } = "";
        public string WeaponType { get; set; } = "";
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public double MarketValue { get; set; }
        public int Circulation { get; set; }
        public string ImageUrl { get; set; } = "";

    }
}
