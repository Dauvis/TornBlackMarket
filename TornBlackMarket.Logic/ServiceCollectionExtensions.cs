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
            services.AddSingleton<IProfileService, ProfileService>();
            services.AddSingleton<IItemService, ItemService>();
            services.AddHttpClient<ITornApiService, TornApiService>();
            services.AddSingleton<IExchangeService, ExchangeService>();

            return services;
        }
    }
}
