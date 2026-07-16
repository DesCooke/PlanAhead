using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Interfaces;
using HomeBudget.Models;
using HomeBudget.Repositories;

namespace HomeBudget.ViewModels;

public partial class DeveloperToolsViewModel : ObservableObject
{
    private readonly AccountRepository _repository;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    public DeveloperToolsViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs)
    {
        _repository = repository;
        _navigation = navigation;
        _dialogs = dialogs;
    }

    [RelayCommand]
    private async Task CreateTestAccount()
    {
        await _repository.AddAsync(new Account
        {
            Name = $"Current Account {DateTime.Now:HHmmss}",
            Balance = 1000m,
            DisplayOrder = 1
        });

        await _dialogs.ShowMessageAsync(
                "Developer", "Test account created.");
    }

    [RelayCommand]
    private async Task ShowAllAccounts()
    {
        var accounts = await _repository.GetAllAsync();

        if (accounts.Count == 0)
        {
            await _dialogs.ShowMessageAsync(
                "Accounts","No accounts found.");

            return;
        }

        var text = string.Join(
            Environment.NewLine,
            accounts.Select(a =>
                $"{a.Name}   {a.Balance:C}"));

        await _dialogs.ShowMessageAsync(
                "Accounts",$"{text}");
    }

    [RelayCommand]
    private async Task DeleteAllAccounts()
    {
        var accounts = await _repository.GetAllAsync();

        foreach (var account in accounts)
            await _repository.DeleteAsync(account);

        await _dialogs.ShowMessageAsync(
                "Developer","All accounts deleted.");
    }
    [RelayCommand]
    private async Task ShowAccountCount()
    {
        var accounts = await _repository.GetAllAsync();

        await _dialogs.ShowMessageAsync(
                "Accounts",$"There are {accounts.Count} accounts.");
    }

    [RelayCommand]
    private async Task ShowDatabasePath()
    {
        var path = Path.Combine(
            FileSystem.AppDataDirectory,
            "homebudget.db");

        await _dialogs.ShowMessageAsync(
                "Database",$"{path}");
    }
}