using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;
using System.Collections.ObjectModel;

namespace HomeBudget.ViewModels;

public partial class AccountsViewModel : BaseViewModel
{
    private readonly AccountRepository _repository;
    private readonly INavigationContext _context;

    [ObservableProperty]
    private Account? selectedAccount;

    public ObservableCollection<Account> Accounts { get; } = new();

    public AccountsViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs,
        INavigationContext context): base(navigation, dialogs)
    {
        _repository = repository;
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
        await Navigation.GoToAccountDetailAsync();
    }

    [RelayCommand]
    private async Task AddAccount()
    {
        await Navigation.GoToAccountDetailAsync();
    }

    partial void OnSelectedAccountChanged(Account? value)
    {
        if (value == null)
            return;

        EditAccountCommand.Execute(value);

        SelectedAccount = null;
    }
}