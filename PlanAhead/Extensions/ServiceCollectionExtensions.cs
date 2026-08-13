
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Data.Database;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Services;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels;
using PlanAhead.ViewModels.Accounts;
using PlanAhead.ViewModels.Funds;
using PlanAhead.ViewModels.Icons;
using PlanAhead.Views;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;
using PlanAhead.Views.Popups;
using PlanAhead.Views.Startup;

namespace PlanAhead.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlanAhead(
            this IServiceCollection services)
        {
            // System
            services.AddSingleton<DatabaseStartup>();
            services.AddSingleton<AppShell>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INavigationContext, NavigationContext>();
            services.AddSingleton<ISupabaseClientProvider, SupabaseClientProvider>();
            services.AddSingleton<IApplicationSettingsService,
                      ApplicationSettingsService>();
            services.AddSingleton<IApplicationStartupService, ApplicationStartupService>();
            services.AddSingleton<AppShell>();

            services.AddSingleton<IApplicationStartupService, ApplicationStartupService>();

            // Services 
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IconPickerViewModel>();
            services.AddTransient<IconPickerPopup>();
            services.AddTransient<IAccountHealthService, AccountHealthService>();

            // Repositories
            services.AddTransient<AccountRepository>();

            // ViewModels and Pages
            services.AddTransient<AccountEditPage>();
            services.AddTransient<AccountEditViewModel>();

            services.AddTransient<AccountViewPage>();
            services.AddTransient<AccountViewViewModel>();

            services.AddTransient<AccountsPage>();
            services.AddTransient<AccountsViewModel>();

            services.AddTransient<DashboardPage>();
            services.AddTransient<DashboardViewModel>();

            services.AddTransient<DeveloperToolsPage>();
            services.AddTransient<DeveloperToolsViewModel>();

            services.AddTransient<FundEditPage>();
            services.AddTransient<FundEditViewModel>();

            services.AddTransient<FundsPage>();
            services.AddTransient<FundsViewModel>();

            services.AddTransient<FundViewPage>();
            services.AddTransient<FundViewViewModel>();

            services.AddTransient<WelcomePage>();
            services.AddTransient<WelcomeViewModel>();

            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();

            return services;
        }
    }
}
