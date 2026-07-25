
using PlanAhead.Data.Database;
using PlanAhead.Infrastructure.Data.Database;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels;
using PlanAhead.ViewModels.Accounts;
using PlanAhead.ViewModels.Funds;
using PlanAhead.ViewModels.Icons;
using PlanAhead.Views;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;

namespace PlanAhead.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlanAhead(
            this IServiceCollection services)
        {
            // System
            services.AddSingleton<ApplicationStartupService>();
            services.AddSingleton<AppShell>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INavigationContext, NavigationContext>();

            // Services 
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IconPickerViewModel>();
            services.AddTransient<IconPickerPopup>();

            // Repositories
            services.AddTransient<AccountRepository>();

            // ViewModels and Pages
            services.AddTransient<AccountDetailPage>();
            services.AddTransient<AccountDetailViewModel>();

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

            return services;
        }
    }
}
