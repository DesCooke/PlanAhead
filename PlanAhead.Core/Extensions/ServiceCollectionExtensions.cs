using Microsoft.Extensions.DependencyInjection;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Core.Services.FundingRules;
using PlanAhead.Core.Services.Funds;
using PlanAhead.Core.Services.Planning;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlanAheadCore(
            this IServiceCollection services)
        {
            services.AddSingleton<IAccountService, AccountService>();

            services.AddSingleton<IFundService, FundService>();

            services.AddSingleton<IFundingRuleService, FundingRuleService>();

            services.AddSingleton<IForecastEngine, ForecastEngine>();

            services.AddSingleton<IFundingProjectionService, FundingProjectionService>();

            services.AddSingleton<ILedgerProjectionService, LedgerProjectionService>();

            return services;
        }
    }
}
