using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411161040, "Create UserProfileDocumentTable")]
    public class CreateUserProfileDocumentTable : Migration
    {
        public override void Down()
        {
            Log.Information("Removing table {Table}", Constants.UserProfileDocument);
            Delete.Table(Constants.UserProfileDocument);
            Log.Information("Table {Table} removed", Constants.UserProfileDocument);
        }

        public override void Up()
        {
            Log.Information("Creating table {Table}", Constants.UserProfileDocument);
            Create.Table(Constants.UserProfileDocument)
                .WithColumn("Id").AsString(20).PrimaryKey()
                .WithColumn("Name").AsString(50)
                .WithColumn("ApiKey").AsString(50);
            Log.Information("Table {Table} created", Constants.UserProfileDocument);
        }
    }
}
