using CommunityToolkit.Maui;
using HomeBudget;
using HomeBudget.ViewModels;
using HomeBudget.Views;
using Microsoft.Extensions.Logging;
using PlanAhead.Data.Database;
using PlanAhead.Core.Interfaces;
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
