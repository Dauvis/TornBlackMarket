using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411191247, "Add fields to UserProfileDocument for bazaar support")]
    public class Migration_2411191247_AlterProfileDocumentForContact : Migration
    {
        public override void Down()
        {
            Log.Information("Removing bazaar columns from {Table}", Constants.Profiles);
            Delete.Column("Email").FromTable(Constants.Profiles);
            Delete.Column("Web").FromTable(Constants.Profiles);
            Delete.Column("Discord").FromTable(Constants.Profiles);
            Log.Information("Finished removing columns from {Table}", Constants.Profiles);
        }

        public override void Up()
        {
            Log.Information("Adding bazaar columns to {Table}", Constants.Profiles);
            Alter.Table(Constants.Profiles)
                .AddColumn("Email").AsString(250).WithDefaultValue("")
                .AddColumn("Web").AsString(250).WithDefaultValue("")
                .AddColumn("Discord").AsString(250).WithDefaultValue("");
            Log.Information("Finished adding columns to {Table}", Constants.Profiles);
        }
    }
}
