using Dapper.Contrib.Extensions;

namespace TornBlackMarket.Data.Models
{
    [Table("UserProfileDocument")]
    public class UserProfileDocument
    {
        [ExplicitKey]
        public string Id { get; set; } = "";
        public string ApiKey { get; set; } = "";
        public string Name { get; set; } = "";

    }
}
