using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TornBlackMarket.Periodic.Enums;
using TornBlackMarket.Periodic.Interfaces;

namespace TornBlackMarket.Periodic.Services
{
    public class JobsService
    {
        private readonly Dictionary<JobIdType, Type> _jobIdToInterfaceMap = new()
        { 
            {JobIdType.ItemLoad, typeof(IItemLoadJobService)}
        };

        private readonly JobSettings _jobSettings;
        private readonly IServiceProvider _services;
        private readonly ILogger<JobsService> _logger;

        public JobsService(JobSettings jobSettings, IServiceProvider services, ILogger<JobsService> logger) 
        {
            _jobSettings = jobSettings;
            _services = services;
            _logger = logger;
        }

        public void Execute()
        {
            foreach (var jobId in _jobSettings.JobIdList)
            {
                var jobInterface = _jobIdToInterfaceMap[jobId];
                JobServiceBase jobService = (JobServiceBase)_services.GetRequiredService(jobInterface);
                
                try
                {
                    _logger.LogInformation("Starting job: {JobId}", jobId);
                    Task.Run(async () => await jobService.ExecuteAsync(_jobSettings)).Wait();
                    _logger.LogInformation("Finished job: {JobId}", jobId);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error in running {JobId}: {Message}", jobId, e.Message);
                }
            }
        }
    }
}
