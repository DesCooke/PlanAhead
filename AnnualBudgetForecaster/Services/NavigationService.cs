using HomeBudget.Interfaces;
using HomeBudget.Views;

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

    public async Task GoToAccountDetailAsync(Guid? accountId = null)
    {
        if (accountId == null)
        {
            await Shell.Current.GoToAsync(nameof(AccountDetailPage));
        }
        else
        {
            await Shell.Current.GoToAsync(
                $"{nameof(AccountDetailPage)}?id={accountId}");
        }
    }
}