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
    private async Task ShowAllAccounts()
    {
        try
        {
            var accounts = await _repository.GetAllAsync();

            if (accounts.Count == 0)
            {
                await Dialogs.ShowMessageAsync(
                    "Accounts", "No accounts found.");

                return;
            }
        }
        catch (Exception ex)
        {
            var msg = $"Error in DeveloperToolsViewModel:ShowAllAccounts:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task DeleteAll()
    {
        try
        {
            var path = Path.Combine(
                FileSystem.AppDataDirectory,
                "PlanAhead.db");
            File.Delete(path);
            Preferences.Default.Clear();
            await Dialogs.ShowMessageAsync(
                    "Set as New Install", $"Database and Preferences removed. Next run will be as a New Install");
        }
        catch (Exception ex)
        {
            var msg = $"Error in DeveloperToolsViewModel:DeleteAll:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }


    }

    [RelayCommand]
    private async Task ShowAccountCount()
    {
        try
        {
            var accounts = await _repository.GetAllAsync();

            await Dialogs.ShowMessageAsync(
                    "Accounts", $"There are {accounts.Count} accounts.");
        }
        catch (Exception ex)
        {
            var msg = $"Error in DeveloperToolsViewModel:ShowAccountCount:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task ShowDatabasePath()
    {
        try
        {
            var path = Path.Combine(
                FileSystem.AppDataDirectory,
                "PlanAhead.db");

            await Dialogs.ShowMessageAsync(
                    "Database", $"{path}");
        }
        catch (Exception ex)
        {
            var msg = $"Error in DeveloperToolsViewModel:ShowDatabasePath:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }
}