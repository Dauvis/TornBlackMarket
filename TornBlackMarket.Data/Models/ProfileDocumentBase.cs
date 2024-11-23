using Dapper.Contrib.Extensions;

namespace TornBlackMarket.Data.Models
{
    [Table("Profiles")]
    public class ProfileDocumentBase
    {
        [ExplicitKey]
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Web { get; set; } = "";
        public string Discord { get; set; } = "";
        public DateTimeOffset TokenInvalidDateTime { get; set; } = DateTimeOffset.MinValue;
    }
}
