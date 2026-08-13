using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;

namespace PlanAhead.ViewModels;

public partial class DeveloperToolsViewModel : BaseViewModel
{
    private readonly AccountRepository _repository;

    public DeveloperToolsViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs): base (navigation, dialogs)
    {
        _repository = repository;
    }

    [RelayCommand]
    private async Task CreateTestAccount()
    {
//        await _repository.AddAsync(new Account
  //      {
    //        Name = $"Current Account {DateTime.Now:HHmmss}",
      //      Balance = 1000m,
        //    DisplayOrder = 1
//        });

        await Dialogs.ShowMessageAsync(
                "Developer", "Test account created.");
    }

    [RelayCommand]
    private async Task ShowAllAccounts()
    {
        var accounts = await _repository.GetAllAsync();

        if (accounts.Count == 0)
        {
            await Dialogs.ShowMessageAsync(
                "Accounts","No accounts found.");

            return;
        }

//        var text = string.Join(
  //          Environment.NewLine,
    //        accounts.Select(a =>
      //          $"{a.Name}   {a.Balance:C}"));

        //await Dialogs.ShowMessageAsync(
          //      "Accounts",$"{text}");
    }

    [RelayCommand]
    private async Task DeleteAll()
    {
        var path = Path.Combine(
            FileSystem.AppDataDirectory,
            "PlanAhead.db");
        File.Delete(path);
        Preferences.Default.Clear();
        await Dialogs.ShowMessageAsync(
                "Set as New Install", $"Database and Preferences removed. Next run will be as a New Install");

    }

    [RelayCommand]
    private async Task ShowAccountCount()
    {
        var accounts = await _repository.GetAllAsync();

        await Dialogs.ShowMessageAsync(
                "Accounts",$"There are {accounts.Count} accounts.");
    }

    [RelayCommand]
    private async Task ShowDatabasePath()
    {
        var path = Path.Combine(
            FileSystem.AppDataDirectory,
            "PlanAhead.db");

        await Dialogs.ShowMessageAsync(
                "Database",$"{path}");
    }
}