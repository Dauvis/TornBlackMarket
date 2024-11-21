using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411211351, "Alter profiles table to add support for API key encryption")]
    public class Migration_2411211351_AlterProfileTableForKeyEncryption : Migration
    {
        public override void Down()
        {
            Log.Information("Alterting {Table}", Constants.Profiles);
            Alter.Table(Constants.Profiles)
                .AlterColumn("ApiKey").AsString(50);
            Delete.Column("ApiKeyVI").FromTable(Constants.Profiles);
            Log.Information("Finished altering {Table}", Constants.Profiles);
        }

        public override void Up()
        {
            Log.Information("Alterting {Table}", Constants.Profiles);
            Alter.Table(Constants.Profiles)
                .AlterColumn("ApiKey").AsString(200)
                .AddColumn("ApiKeyVI").AsBinary().WithDefaultValue(Array.Empty<byte>());
            Log.Information("Finished altering {Table}", Constants.Profiles);
        }
    }
}
