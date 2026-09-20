using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Logging;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;

namespace PlanAhead.ViewModels;

public partial class DeveloperToolsViewModel : BaseViewModel
{
    private readonly AccountRepository _repository;

    public DeveloperToolsViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs,
        ILogService logService)
        : base(navigation, dialogs, logService)
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
                await DialogService.ShowMessageAsync(
                    "Accounts", "No accounts found.");

                return;
            }
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
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
            await DialogService.ShowMessageAsync(
                    "Set as New Install", $"Database and Preferences removed. Next run will be as a New Install");
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
        }
    }

    [RelayCommand]
    private async Task ShowAccountCount()
    {
        try
        {
            var accounts = await _repository.GetAllAsync();

            await DialogService.ShowMessageAsync(
                    "Accounts", $"There are {accounts.Count} accounts.");
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
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

            await DialogService.ShowMessageAsync(
                    "Database", $"{path}");
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
        }
    }
}