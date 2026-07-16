namespace HomeBudget.Interfaces;

public interface INavigationService
{
    Task GoBackAsync();

    Task GoToAsync(string route);
}