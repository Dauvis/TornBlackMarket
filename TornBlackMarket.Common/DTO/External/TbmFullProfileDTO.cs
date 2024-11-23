using TornBlackMarket.Common.Enums;

namespace TornBlackMarket.Common.DTO.External
{
    public class TbmFullProfileDTO
    {
        public TbmProfileDTO BasicProfile { get; set; } = new();
        public TbmExchangeDTO Exchange { get; set; } = new();
    }
}
