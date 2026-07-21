using CommunityToolkit.Maui;
using HomeBudget;
using HomeBudget.ViewModels;
using HomeBudget.Views;
using Microsoft.Extensions.Logging;
using PlanAhead.Core.Interfaces;
using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Dates.Strategies;
using PlanAhead.Core.Services.Planning;
using PlanAhead.Core.Extensions;
using PlanAhead.Data.Database;
using PlanAhead.Services;
using PlanAhead.Infrastructure.Data.Database;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Extensions;

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

            builder.Services.AddSingleton<IDialogService,
                DialogService>();

            builder.Services.AddSingleton<INavigationContext,
                              NavigationContext>();
            builder.Services.AddSingleton<BaseFrequencyStrategy, MonthlyFrequencyStrategy>();

            builder.Services.AddSingleton<BaseFrequencyStrategy, QuarterlyFrequencyStrategy>();

            builder.Services.AddSingleton<BaseFrequencyStrategy, BiAnnualFrequencyStrategy>();

            builder.Services.AddSingleton<BaseFrequencyStrategy, AnnualFrequencyStrategy>();

            builder.Services.AddSingleton<BaseFrequencyStrategy, OneOffFrequencyStrategy>();

            builder.Services.AddPlanAheadCore();
            builder.Services.AddPlanAheadInfrastructure(
                Path.Combine(
                    FileSystem.AppDataDirectory,
                    "planahead.db"));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
