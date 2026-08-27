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
        Routes[typeof(TPage)] = route;

        Routing.RegisterRoute(
            route,
            typeof(TPage));
    }

    public static string GetRoute<TPage>()
        where TPage : Page
    {
        if (!Routes.TryGetValue(
                typeof(TPage),
                out var route))
        {
            throw new InvalidOperationException(
                $"No route registered for {typeof(TPage).Name}");
        }

        return route;
    }

    public static void RegisterRoutes()
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
}