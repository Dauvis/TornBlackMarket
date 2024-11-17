using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TornBlackMarket.Migrations;

var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

var settings = CommandLineUtil.GetSettings(configuration, args);
var serviceProvider = CreateServices(settings.ConnectionString);

using (var scope = serviceProvider.CreateScope())
{
    if (settings.DowngradeVersion == 0)
    {
        MigrationUtil.UpgradeDatabase(scope.ServiceProvider);
    }
    else
    {
        MigrationUtil.DowngradeDatabase(scope.ServiceProvider, settings.DowngradeVersion);
    }
}

static IServiceProvider CreateServices(string connectionString)
{
    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddSqlServer()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(Program).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}
