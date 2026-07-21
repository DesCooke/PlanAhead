using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Planning;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace PlanAhead.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlanAheadCore(
            this IServiceCollection services)
        {
            services.AddSingleton<IFundingProjectionService, FundingProjectionService>();

            services.AddSingleton<IForecastEngine, ForecastEngine>();

            return services;
        }
    }
}
