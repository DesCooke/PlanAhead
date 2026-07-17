using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Constants;
using HomeBudget.Interfaces;
using HomeBudget.Models;
using HomeBudget.Repositories;

namespace HomeBudget.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    [ObservableProperty]
    private string title = AppConstants.ApplicationName;

    [ObservableProperty]
    private string welcomeMessage =
        "Welcome to Home Budget";

    [ObservableProperty]
    private string version =
        $"Version {AppConstants.Version}";

    private readonly AccountRepository _repository;

    public DashboardViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs): base (navigation, dialogs)
    {
        _repository = repository;
    }

    [RelayCommand]
    private async Task TestRepository()
    {
        var account = new Account
        {
            Name = "Current Account",
            Balance = 1250.00m,
            DisplayOrder = 1
        };

        await _repository.AddAsync(account);

        var accounts = await _repository.GetAllAsync();

        System.Diagnostics.Debug.WriteLine(
            $"Number of accounts = {accounts.Count}");
    }
}