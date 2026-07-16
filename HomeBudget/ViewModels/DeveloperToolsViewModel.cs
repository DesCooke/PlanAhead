using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Models;
using HomeBudget.Repositories;

namespace HomeBudget.ViewModels;

public partial class DeveloperToolsViewModel : ObservableObject
{
    private readonly AccountRepository _repository;

    public DeveloperToolsViewModel(AccountRepository repository)
    {
        _repository = repository;
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

        await Shell.Current.DisplayAlert(
            "Developer",
            "Test account created.",
            "OK");
    }

    [RelayCommand]
    private async Task ShowAllAccounts()
    {
        var accounts = await _repository.GetAllAsync();

        if (accounts.Count == 0)
        {
            await Shell.Current.DisplayAlert(
                "Accounts",
                "No accounts found.",
                "OK");

            return;
        }

        var text = string.Join(
            Environment.NewLine,
            accounts.Select(a =>
                $"{a.Name}   {a.Balance:C}"));

        await Shell.Current.DisplayAlert(
            "Accounts",
            text,
            "OK");
    }

    [RelayCommand]
    private async Task DeleteAllAccounts()
    {
        var accounts = await _repository.GetAllAsync();

        foreach (var account in accounts)
            await _repository.DeleteAsync(account);

        await Shell.Current.DisplayAlert(
            "Developer",
            "All accounts deleted.",
            "OK");
    }
    [RelayCommand]
    private async Task ShowAccountCount()
    {
        var accounts = await _repository.GetAllAsync();

        await Shell.Current.DisplayAlert(
            "Accounts",
            $"There are {accounts.Count} accounts.",
            "OK");
    }

    [RelayCommand]
    private async Task ShowDatabasePath()
    {
        var path = Path.Combine(
            FileSystem.AppDataDirectory,
            "homebudget.db");

        await Shell.Current.DisplayAlert(
            "Database",
            path,
            "OK");
    }
}