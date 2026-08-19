using Microsoft.Extensions.DependencyInjection;
using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.DB.Supabase;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Sync;

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

        services.AddSingleton<ISyncService, SyncService>();

        services.AddSingleton<IEntitySynchroniser, AccountSynchroniser>();

        services.AddSingleton<IEntitySynchroniser, FundSynchroniser>();

        services.AddSingleton<ILocalDatabaseService, LocalDatabaseService>();

        services.AddSingleton<IRemoteDatabaseService, RemoteDatabaseService>();

        return services;
    }
}