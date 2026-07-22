using CommunityToolkit.Maui;
using HomeBudget;
using HomeBudget.ViewModels;
using HomeBudget.Views;
using Microsoft.Extensions.Logging;
using PlanAhead.Core.Extensions;
using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Planning;
using PlanAhead.Data.Database;
using PlanAhead.Infrastructure.Data.Database;
using PlanAhead.Infrastructure.Extensions;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;
using PlanAhead.Navigation;
using PlanAhead.Services;
using PlanAhead.ViewModels.Funds;
using PlanAhead.Views.Funds;

namespace PlanAhead
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DashboardViewModel>();

            builder.Services.AddSingleton<DashboardPage>();

            builder.Services.AddSingleton(
                new SQLiteContext(
                    Path.Combine(
                        FileSystem.AppDataDirectory,
                        "planahead.db")));

            builder.Services.AddSingleton<ApplicationStartupService>();

            builder.Services.AddSingleton<AccountRepository>();

            builder.Services.AddSingleton<DeveloperToolsViewModel>();

            builder.Services.AddSingleton<DeveloperToolsPage>();

            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddSingleton<AccountsViewModel>();

            builder.Services.AddSingleton<AccountDetailViewModel>();

            builder.Services.AddSingleton<AccountsPage>();

            builder.Services.AddSingleton<INavigationService,
                NavigationService>();

            builder.Services.AddTransient<FundListViewModel>();

            builder.Services.AddTransient<FundListPage>();

            builder.Services.AddSingleton<IDialogService,
                DialogService>();

            builder.Services.AddSingleton<INavigationContext,
                              NavigationContext>();

            builder.Services.AddPlanAheadCore();
            builder.Services.AddPlanAheadInfrastructure(
                Path.Combine(
                    FileSystem.AppDataDirectory,
                    "planahead.db"));

            RouteRegistry.Register<FundListPage>(
                "funds");

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
