using Microsoft.Extensions.DependencyInjection;

namespace TornBlackMarket.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            // Add any services specific to the common layer here

            return services;
        }
    }
}
