﻿using Microsoft.Extensions.DependencyInjection;
using TornBlackMarket.Common.Interfaces;
using TornBlackMarket.Security;

namespace TornBlackMarket.Security
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services)
        {
            // Add any services specific to the security layer here
            services.AddSingleton<ITornBlackMarketTokenUtil, TornBlackMarketTokenUtil>();
            services.AddSingleton<ITornApiKeyUtil, TornApiKeyUtil>();
            services.AddSingleton<IEncryptionUtil, EncryptionUtil>();

            return services;
        }
    }
}
