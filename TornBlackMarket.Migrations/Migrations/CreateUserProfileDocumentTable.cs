using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411161040, "Create UserProfileDocumentTable")]
    public class CreateUserProfileDocumentTable : Migration
    {
        public override void Down()
        {
            Log.Information("Removing table {Table}", Constants.Profiles);
            Delete.Table(Constants.Profiles);
            Log.Information("Table {Table} removed", Constants.Profiles);
        }

        public override void Up()
        {
            Log.Information("Creating table {Table}", Constants.Profiles);
            Create.Table(Constants.Profiles)
                .WithColumn("Id").AsString(20).PrimaryKey()
                .WithColumn("Name").AsString(50)
                .WithColumn("ApiKey").AsString(50);
            Log.Information("Table {Table} created", Constants.Profiles);
        }
    }
}
