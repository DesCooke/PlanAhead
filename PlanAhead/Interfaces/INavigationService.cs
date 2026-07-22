namespace PlanAhead.Interfaces;

public interface INavigationService
{
    Task NavigateToAsync<TPage>()
        where TPage : Page;

    Task NavigateToAsync<TPage>(
        IDictionary<string, object> parameters)
        where TPage : Page;

    Task GoBackAsync();

    Task GoToRootAsync();
}