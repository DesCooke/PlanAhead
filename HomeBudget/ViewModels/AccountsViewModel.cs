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
    private readonly INavigationContext _context;

    [ObservableProperty]
    private Account? selectedAccount;

    public ObservableCollection<Account> Accounts { get; } = new();

    public AccountsViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs,
        INavigationContext context)
    {
        _repository = repository;
        _navigation = navigation;
        _dialogs = dialogs;
        _context = context;
    }

    public async Task LoadAsync()
    {
        Accounts.Clear();

        var accounts = await _repository.GetAllAsync();

        foreach (var account in accounts)
            Accounts.Add(account);
    }

    [RelayCommand]
    private async Task EditAccount(Account account)
    {
        _context.Set(account); 
        await _navigation.GoToAccountDetailAsync();
    }

    [RelayCommand]
    private async Task AddAccount()
    {
        await _navigation.GoToAccountDetailAsync();
    }

    partial void OnSelectedAccountChanged(Account? value)
    {
        if (value == null)
            return;

        EditAccountCommand.Execute(value);

        SelectedAccount = null;
    }
}