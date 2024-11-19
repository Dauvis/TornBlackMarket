using TornBlackMarket.Data;
using TornBlackMarket.Logic;
using Microsoft.Extensions.DependencyInjection;
using TornBlackMarket.Periodic.Interfaces;
using TornBlackMarket.Periodic.Services;

namespace TornBlackMarket.Periodic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPeriodicServices(this IServiceCollection services)
        {
            services.AddSingleton<IItemLoadJobService, ItemLoadJobService>();

            // Add AutoMapper with the profiles in your assembly
            services.AddAutoMapper(typeof(ServiceMappingProfile).Assembly);
            services.AddAutoMapper(typeof(DataMappingProfile).Assembly);            

            return services;
        }
    }
}
