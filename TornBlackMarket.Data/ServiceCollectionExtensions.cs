using Microsoft.Extensions.DependencyInjection;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Data.Abstraction;
using TornBlackMarket.Data.Interfaces;

namespace TornBlackMarket.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            // Add any services specific to the data layer here
            services.AddSingleton<IDataStoreClient, DataStoreClient> ();
            services.AddSingleton<IRepositoryFactory, RepositoryFactory> ();

            return services;
        }
    }
}
