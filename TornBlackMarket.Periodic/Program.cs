using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TornBlackMarket.Periodic.Util;
using TornBlackMarket.Common;
using TornBlackMarket.Data;
using TornBlackMarket.Logic;
using TornBlackMarket.Periodic.Services;
using Microsoft.Extensions.Logging;
using TornBlackMarket.Periodic;

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
var services = CreateServices(configuration);

var jobServiceLogger = services.GetRequiredService<ILogger<JobsService>>();
var jobService = new JobsService(settings, services, jobServiceLogger);
jobService.Execute();

static IServiceProvider CreateServices(IConfiguration configuration)
{
    var services = new ServiceCollection();

    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddSerilog(dispose: true);
    });

    services.AddSingleton(configuration);
    services.AddCommonServices();
    services.AddDataServices();
    services.AddLogicServices();
    services.AddPeriodicServices();

    return services.BuildServiceProvider();
}
