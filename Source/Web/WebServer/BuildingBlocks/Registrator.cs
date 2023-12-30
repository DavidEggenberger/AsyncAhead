﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Web.Server.BuildingBlocks.ContextAccessor.ExecutionContextAccessor;
using Web.Server.BuildingBlocks.ContextAccessor.WebContextAccessor;
using Web.Server.BuildingBlocks.HostingInformation;
using Web.Server.BuildingBlocks.Logging;
using Web.Server.BuildingBlocks.ModelValidation;
using Web.Server.BuildingBlocks.ResponseCompression;
using Web.Server.BuildingBlocks.Swagger;
using WebServer.Modules.ModelValidation;
using WebServer.Modules.Swagger;

namespace Web.Server.BuildingBlocks
{
    public static class Registrator
    {
        public static IServiceCollection AddBuildingBlocks(this IServiceCollection services)
        {
            services.RegisterAntiforgeryToken();
            services.RegisterApiVersioning();
            services.RegisterLogging();
            services.RegisterModelValidation();
            services.RegisterSwagger();
            services.RegisterWebContextAccessor();
            services.RegisterExecutionContextAccessor();
            services.RegisterModelValidation();
            services.RegisterResponseCompression();

            return services;
        }

        public static IApplicationBuilder UseBuildingBlocks(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.RegisterApiVersioning();
            applicationBuilder.RegisterExceptionHandling();
            applicationBuilder.RegisterLogging();
            applicationBuilder.RegisterSecurityHeaders();
            applicationBuilder.RegisterSwagger();
            applicationBuilder.RegisterExecutionContextAccessingMiddleware();
            applicationBuilder.UserResponseCompression();

            return applicationBuilder;
        }
    }
}
