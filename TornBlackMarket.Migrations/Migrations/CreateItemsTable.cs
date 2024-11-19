using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411171744, "Create Items table")]
    public class CreateItemsTable : Migration
    {
        public override void Down()
        {
            Log.Information("Removing table {Table}", Constants.Items);
            Delete.Table(Constants.Items);
            Log.Information("Table {Table} removed", Constants.Items);
        }

        public override void Up()
        {
            Log.Information("Creating table {Table}", Constants.Items);
            Create.Table(Constants.Items)
                .WithColumn("Id").AsString(20).PrimaryKey()
                .WithColumn("Name").AsString(50)
                .WithColumn("Description").AsString(2000)
                .WithColumn("Effect").AsString(250)
                .WithColumn("Requirement").AsString(250)
                .WithColumn("Type").AsString(50)
                .WithColumn("WeaponType").AsString(50).Nullable()
                .WithColumn("BuyPrice").AsDouble()
                .WithColumn("SellPrice").AsDouble()
                .WithColumn("MarketValue").AsDouble()
                .WithColumn("Circulation").AsInt32()
                .WithColumn("ImageUrl").AsString(250);
            Log.Information("Table {Table} created", Constants.Items);

        }
    }
}
