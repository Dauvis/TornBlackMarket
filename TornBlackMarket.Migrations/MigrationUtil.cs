using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TornBlackMarket.Migrations
{
    internal class MigrationUtil
    {
        public static void UpgradeDatabase(IServiceProvider serviceProvider)
        {
            Log.Information("Peforming database upgrade");
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
            Log.Information("Database upgrade finished");
        }

        public static void DowngradeDatabase(IServiceProvider serviceProvider, long downgradeVersion)
        {
            Log.Information("Downgrading database to version {Version}", downgradeVersion);
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateDown(downgradeVersion);
            Log.Information("Database downgrade finished");
        }
    }
}
