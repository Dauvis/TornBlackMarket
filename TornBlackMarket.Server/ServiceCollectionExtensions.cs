using TornBlackMarket.Data;
using TornBlackMarket.Logic;
using Serilog;

namespace TornBlackMarket.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services)
        {

            // Add AutoMapper with the profiles in your assembly
            services.AddAutoMapper(typeof(ServerMappingProfile).Assembly);
            services.AddAutoMapper(typeof(ServiceMappingProfile).Assembly);
            services.AddAutoMapper(typeof(DataMappingProfile).Assembly);            

            return services;
        }
    }
}
