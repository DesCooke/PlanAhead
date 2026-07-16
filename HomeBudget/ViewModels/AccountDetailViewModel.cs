using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.Interfaces;
using HomeBudget.Models;
using HomeBudget.Repositories;

namespace HomeBudget.ViewModels;

public partial class AccountDetailViewModel : ObservableObject
{
    private readonly AccountRepository _repository;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;
    private Account? _editingAccount;

    public AccountDetailViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs)
    {
        _repository = repository;
        _navigation = navigation;
        _dialogs = dialogs;
    }

    [ObservableProperty]
    private string accountName = "";

    [ObservableProperty]
    private decimal balance;

    [ObservableProperty]
    private bool includeInTotal = true;

    public async Task LoadAsync(Account? account)
    {
        _editingAccount = account;

        if (account == null)
            return;

        AccountName = account.Name;
        Balance = account.Balance;
        IncludeInTotal = account.IncludeInTotal;
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await _navigation.GoBackAsync();
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(AccountName))
        {
            await _dialogs.ShowErrorAsync(
                "Please enter an account name.");

            return;
        }

        if (_editingAccount == null)
        {
            await _repository.AddAsync(
                new Account
                {
                    Name = AccountName,
                    Balance = Balance,
                    IncludeInTotal = IncludeInTotal
                });
        }
        else
        {
            _editingAccount.Name = AccountName;
            _editingAccount.Balance = Balance;
            _editingAccount.IncludeInTotal = IncludeInTotal;

            await _repository.UpdateAsync(_editingAccount);
        }

        await _navigation.GoBackAsync();
    }

}