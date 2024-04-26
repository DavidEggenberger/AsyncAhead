﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Features.Messaging;
using Shared.Features.EFCore;
using Shared.Features.Modules;
using Shared.Features.Server.ExecutionContext;

namespace Shared.Features
{
    public static class Registrator
    {
        public static IServiceCollection AddSharedFeatures(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.AddMessaging();
            services.AddEFCore(configuration);
            services.AddServerExecutionContext();

            return services;
        }

        public static IApplicationBuilder UseSharedFeaturesMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEFCoreMiddleware();
            app.UseServerExecutionContextMiddleware();
            app.UseModulesMiddleware(env);

            return app;
        }
    }
}
