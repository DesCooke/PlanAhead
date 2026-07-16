using HomeBudget.Interfaces;

namespace HomeBudget.Services;

public class NavigationService
    : INavigationService
{
    public Task GoBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }

    public Task GoToAsync(string route)
    {
        return Shell.Current.GoToAsync(route);
    }
}