﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Features.EFCore
{
    public static class DbContextRegistrator
    {
        public static void RegisterDbContext<T>(this IServiceCollection services, string schemaName) where T : DbContext
        {
            services.AddDbContext<T>();
        }
    }
}
