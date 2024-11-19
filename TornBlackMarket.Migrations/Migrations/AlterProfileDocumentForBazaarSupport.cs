using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411191247, "Add fields to UserProfileDocument for bazaar support")]
    public class AlterProfileDocumentForBazaarSupport : Migration
    {
        public override void Down()
        {
            Log.Information("Removing bazaar columns from {Table}", Constants.UserProfileDocument);
            Delete.Column("BazaarActive").FromTable(Constants.UserProfileDocument);
            Delete.Column("UpdateFrequency").FromTable(Constants.UserProfileDocument);
            Log.Information("Finished removing columns from {Table}", Constants.UserProfileDocument);
        }

        public override void Up()
        {
            Log.Information("Adding bazaar columns to {Table}", Constants.UserProfileDocument);
            Alter.Table(Constants.UserProfileDocument)
                .AddColumn("BazaarActive").AsBoolean().WithDefaultValue(false)
                .AddColumn("UpdateFrequency").AsInt32().WithDefaultValue(0);
            Log.Information("Finished adding columns to {Table}", Constants.UserProfileDocument);
        }
    }
}
