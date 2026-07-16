using CommunityToolkit.Maui;
using HomeBudget.Data.Database;
using HomeBudget.Repositories;
using HomeBudget.Services;
using HomeBudget.ViewModels;
using HomeBudget.Views;
using Microsoft.Extensions.Logging;

namespace HomeBudget
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

            builder.Services.AddSingleton<AccountsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
