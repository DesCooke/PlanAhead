using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Constants;
using HomeBudget.Repositories;
using HomeBudget.Models;

namespace HomeBudget.ViewModels;

public partial class DashboardViewModel : ObservableObject
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

    public DashboardViewModel(AccountRepository repository)
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