namespace TornBlackMarket.Migrations
{
    public record MigrationSettings(string ConnectionString, long DowngradeVersion);
}