using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.Views.Accounts;
using System.Collections.ObjectModel;
using PlanAhead.Core.Models.Domain;

namespace PlanAhead.ViewModels.Accounts;
    
public partial class AccountsViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly INavigationContext _navigationContext;
    private readonly IAccountHealthService _accountHealthService;

    public ObservableCollection<AccountListItem> Accounts { get; } = new();

    public bool HasAccounts => Accounts.Any();
    public bool HasNoAccounts => !HasAccounts;


    [ObservableProperty]
    private AccountListItem? selectedAccountListItem;

    public AccountsViewModel(
        IAccountService accountService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IAccountHealthService accountHealthService,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _accountService = accountService;
        _navigationContext = navigationContext;
        _accountHealthService = accountHealthService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        await ExecuteBusyAsync(async () =>
        {
            var accounts = await _accountService.GetAllAsync();

            Accounts.Clear();

            foreach (var account in accounts)
            {
                var status = await _accountHealthService.GetStatusAsync(account);

                Accounts.Add(new AccountListItem
                {
                    Account = account,
                    Status = status
                });
            }

            RefreshUi(
                nameof(HasAccounts),
                nameof(HasNoAccounts),
                nameof(Title));
        });

        Title = $"Accounts ({Accounts.Count})";
    }

    [RelayCommand]
    private async Task DeleteAsync(AccountListItem accountListItem)
    {
        var delete =
            await Dialogs.ConfirmAsync(
                "Delete Account",
                $"Delete '{accountListItem.Account.Name}'?");

        if (!delete)
            return;

        await _accountService.DeleteAsync(accountListItem.Account.Id);

        await LoadAsync();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Navigation.NavigateToAsync<AccountEditPage>();
    }

    [RelayCommand]
    private async Task OpenAsync(AccountListItem accountListItem)
    {
        _navigationContext.Set(accountListItem.Account.Id);

        await Navigation.NavigateToAsync<AccountViewPage>();

    }

    [RelayCommand]
    private async Task EditAsync(AccountListItem accountListItem)
    {
        _navigationContext.Set(accountListItem.Account.Id);

        await Navigation.NavigateToAsync<AccountEditPage>();
    }

}