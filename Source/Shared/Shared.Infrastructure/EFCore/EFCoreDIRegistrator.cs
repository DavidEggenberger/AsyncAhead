﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Features.EFCore
{
    public static class EFCoreDIRegistrator
    {
        public static IServiceCollection RegisterEFCore(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
