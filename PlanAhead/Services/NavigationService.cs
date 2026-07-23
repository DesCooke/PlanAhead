using PlanAhead.Interfaces;
using PlanAhead.Navigation;

public class NavigationService : INavigationService
{
    public Task NavigateToAsync<TPage>()
        where TPage : Page
    {
        return Shell.Current.GoToAsync(
            RouteRegistry.GetRoute<TPage>());
    }

    public Task NavigateToAsync<TPage>(
        IDictionary<string, object> parameters)
        where TPage : Page
    {
        return Shell.Current.GoToAsync(
            RouteRegistry.GetRoute<TPage>(),
            parameters);
    }

    public Task GoBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }

    public Task GoToRootAsync()
    {
        return Shell.Current.GoToAsync("//");
    }
}