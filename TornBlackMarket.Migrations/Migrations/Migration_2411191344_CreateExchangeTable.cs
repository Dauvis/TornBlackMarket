using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411191344, "Create Exchange table")]
    public class Migration_2411191344_CreateExchangeTable : Migration
    {
        public override void Down()
        {
            Log.Information("Removing table {Table}", Constants.Exchanges);
            Delete.Table(Constants.Exchanges);
            Log.Information("Finished removing table {Table}", Constants.Exchanges);
        }

        public override void Up()
        {
            Log.Information("Creating table {Table}", Constants.Exchanges);
            Create.Table(Constants.Exchanges)
                .WithColumn("Id").AsString(20).PrimaryKey()
                .WithColumn("Name").AsString(100)
                .WithColumn("Description").AsString(2000)
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("ShowBazaar").AsBoolean()
                .WithColumn("BazaarRefresh").AsInt32()
                .WithColumn("ShowDisplayCase").AsBoolean()
                .WithColumn("DisplayCaseRefresh").AsInt32();
            Log.Information("Finished creating table {Table}", Constants.Exchanges);
        }
    }
}
