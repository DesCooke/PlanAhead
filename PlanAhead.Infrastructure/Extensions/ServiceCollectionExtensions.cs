using Microsoft.Extensions.DependencyInjection;
using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Database.SQLite;
using PlanAhead.Infrastructure.Repositories;

namespace PlanAhead.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlanAheadInfrastructure(
        this IServiceCollection services,
        string databasePath)
    {
        services.AddSingleton(new SQLiteContext(databasePath));

        services.AddSingleton<IAccountRepository, AccountRepository>();

        services.AddSingleton<IFundRepository, FundRepository>();

        services.AddSingleton<IFundingRuleRepository, FundingRuleRepository>();

        services.AddSingleton<IAuthenticationService, AuthenticationService>();


        // Later...
        // services.AddSingleton<ILedgerEntryRepository, LedgerEntryRepository>();

        return services;
    }
}