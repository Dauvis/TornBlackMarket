using Microsoft.Extensions.DependencyInjection;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Logic.Services;

namespace TornBlackMarket.Logic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services)
        {
            // Add any services specific to the logic layer here
            services.AddSingleton<IUserProfileService, UserProfileService>();
            services.AddHttpClient<ITornApiService, TornApiService>();

            return services;
        }
    }
}
