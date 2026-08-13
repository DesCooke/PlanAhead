using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Constants;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;

namespace PlanAhead.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    [ObservableProperty]
    private string title = AppConstants.ApplicationName;

    [ObservableProperty]
    private string welcomeMessage =
        "Welcome to Plan Ahead";

    [ObservableProperty]
    private string version =
        $"Version {AppConstants.Version}";

    [ObservableProperty]
    private string syncMethod =
        "<Sync Method>";

    private readonly AccountRepository _repository;
    private readonly IApplicationSettingsService _settings;

    public DashboardViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs, 
        IApplicationSettingsService settings): base (navigation, dialogs)
    {
        _repository = repository;
        _settings = settings;
        syncMethod = settings.SyncMode.ToString();
    }

    [RelayCommand]
    private async Task TestRepository()
    {
//        var account = new Account
  //      {
    //        Name = "Current Account",
      //      Balance = 1250.00m,
        //    DisplayOrder = 1
//        };

  //      await _repository.AddAsync(account);

        var accounts = await _repository.GetAllAsync();

        System.Diagnostics.Debug.WriteLine(
            $"Number of accounts = {accounts.Count}");
    }
}