using Dapper.Contrib.Extensions;
using TornBlackMarket.Common.Enums;

namespace TornBlackMarket.Data.Models
{
    [Table("Exchanges")]
    public class ExchangeDocument
    {
        [ExplicitKey]
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public ExchangeStatusType Status { get; set; } = ExchangeStatusType.Inactive;
        public bool ShowBazaar { get; set; } = false;
        public int BazaarRefresh { get; set; } = 0;
        public bool ShowDisplayCase { get; set; } = false;
        public int DisplayCaseRefresh { get; set; } = 0;

    }
}
