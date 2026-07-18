namespace PlanAhead.Core.Interfaces;

public interface INavigationService
{
    Task GoBackAsync();

    Task GoToAsync(string route);
    Task GoToAccountDetailAsync(Guid? accountId = null);
}