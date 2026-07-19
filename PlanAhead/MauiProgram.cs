using CommunityToolkit.Maui;
using HomeBudget;
using HomeBudget.ViewModels;
using HomeBudget.Views;
using Microsoft.Extensions.Logging;
using PlanAhead.Core.Interfaces;
using PlanAhead.Core.Services.Dates;
using PlanAhead.Core.Services.Dates.Strategies;
using PlanAhead.Core.Services.Planning;
using PlanAhead.Data.Database;
using PlanAhead.Repositories;
using PlanAhead.Services;

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

            builder.Services.AddSingleton<SQLiteContext>();

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

            builder.Services.AddSingleton<IDateCalculator, DateCalculator>();

            builder.Services.AddSingleton<IFundingProjectionService, FundingProjectionService>();
            builder.Services.AddSingleton<IFundingRuleRepository, FundingRuleRepository>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
