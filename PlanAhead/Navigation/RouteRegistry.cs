using PlanAhead.Core.Logging;
using PlanAhead.Views;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;
using PlanAhead.Views.Startup;

namespace PlanAhead.Navigation;

public static class RouteRegistry
{
    private static readonly Dictionary<Type, string> Routes = new();

    public static void Register<TPage>(
        string route)
        where TPage : Page
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            Routes[typeof(TPage)] = route;

            Routing.RegisterRoute(
                route,
                typeof(TPage));
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public static string GetRoute<TPage>()
        where TPage : Page
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            if (!Routes.TryGetValue(
                typeof(TPage),
                out var route))
                return route;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
        return "";
    }

    public static void RegisterRoutes()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            Register<WelcomePage>("WelcomePage");
            Register<LoginPage>("LoginPage");
            Register<DashboardPage>("DashboardPage");
            Register<AccountEditPage>("AccountEditPage");
            Register<AccountsPage>("AccountsPage");
            Register<AccountViewPage>("AccountViewPage");
            Register<FundEditPage>("FundEditPage");
            Register<FundsPage>("FundsPage");
            Register<FundViewPage>("FundViewPage");
            Register<DiagnosticsPage>("DiagnosticsPage");
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
}