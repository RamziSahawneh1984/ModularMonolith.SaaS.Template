﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Server.BuildingBlocks.Logging
{
    public static class LoggingDIRegistrator
    {
        public static IServiceCollection RegisterLoggingModule(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddHttpLogging(options =>
            {
            });
        }
    }
}