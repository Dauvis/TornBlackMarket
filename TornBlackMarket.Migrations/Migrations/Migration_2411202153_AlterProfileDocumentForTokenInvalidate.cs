using FluentMigrator;
using Serilog;

namespace TornBlackMarket.Migrations.Migrations
{
    [Migration(2411202153, "Add TokenInvalidDateTime column to profiles table")]
    public class Migration_2411202153_AlterProfileDocumentForTokenInvalidate : Migration
    {
        public override void Down()
        {
            Log.Information("Remove {Column} from {Table}", "TokenInvalidDateTime", Constants.Profiles);
            Delete.Column("TokenInvalidDateTime").FromTable(Constants.Profiles);
            Log.Information("Finished alter table {Table}", Constants.Profiles);
        }

        public override void Up()
        {
            Log.Information("Adding {Column} to {Table}", "TokenInvalidDateTime", Constants.Profiles);
            Alter.Table(Constants.Profiles)
                .AddColumn("TokenInvalidDateTime").AsDateTimeOffset().WithDefaultValue(DateTimeOffset.MinValue);
            Log.Information("Finished altering table {Table}", Constants.Profiles);
        }
    }
}
