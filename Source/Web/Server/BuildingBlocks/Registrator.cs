﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Web.Server.BuildingBlocks.APIVersioning;
using Web.Server.BuildingBlocks.ExceptionHandling;
using Web.Server.BuildingBlocks.Logging;
using Web.Server.BuildingBlocks.ModelValidation;
using Web.Server.BuildingBlocks.SecurityHeaders;
using Web.Server.BuildingBlocks.Swagger;
using WebServer.Modules.AntiforgeryToken;

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
            services.RegisterModelValidation();

            return services;
        }

        public static IApplicationBuilder UseBuildingBlocks(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.RegisterApiVersioning();
            applicationBuilder.RegisterExceptionHandling();
            applicationBuilder.RegisterLogging();
            applicationBuilder.RegisterSecurityHeaders();
            applicationBuilder.RegisterSwagger();

            return applicationBuilder;
        }
    }
}
