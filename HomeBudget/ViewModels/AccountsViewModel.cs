using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Interfaces;
using HomeBudget.Models;
using HomeBudget.Repositories;
using System.Collections.ObjectModel;

namespace HomeBudget.ViewModels;

public partial class AccountsViewModel : ObservableObject
{
    private readonly AccountRepository _repository;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    public ObservableCollection<Account> Accounts { get; } = new();

    public AccountsViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs)
    {
        _repository = repository;
        _navigation = navigation;
        _dialogs = dialogs;
    }

    public async Task LoadAsync()
    {
        Accounts.Clear();

        var accounts = await _repository.GetAllAsync();

        foreach (var account in accounts)
            Accounts.Add(account);
    }

    [RelayCommand]
    private async Task AddAccount()
    {
        var account = new Account
        {
            Name = "New Account",
            Balance = 0,
            DisplayOrder = Accounts.Count + 1
        };

        await _repository.AddAsync(account);

        await LoadAsync();
    }
}