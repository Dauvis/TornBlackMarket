using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornBlackMarket.Common.DTO.Domain
{
    public class ItemDocumentDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Effect { get; set; } = "";
        public string Requirement { get; set; } = "";
        public string Type { get; set; } = "";
        public string WeaponType { get;  set; } = "";
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public double MarketValue { get; set; }
        public int Circulation {  get; set; }
        public string ImageUrl { get; set; } = "";
    }
}
