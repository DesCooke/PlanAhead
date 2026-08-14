using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Core.Services.Funds;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.Views.Accounts;
using System.Collections.ObjectModel;

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

    public async Task InitialiseAsync()
    {
        await LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        try
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

            Title = $"Accounts ({Accounts.Count})";
        }
        catch (Exception ex)
        {
            await Dialogs.ShowErrorAsync("FundsViewModal.cs:LoadAsync:Exception:" + ex.ToString());
        }

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

        await InitialiseAsync();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync("AccountEditPage");
    }

    [RelayCommand]
    private async Task OpenAsync(AccountListItem accountListItem)
    {
        _navigationContext.Set(accountListItem.Account.Id);

        await Shell.Current.GoToAsync("AccountViewPage");
    }

    [RelayCommand]
    private async Task EditAsync(AccountListItem accountListItem)
    {
        _navigationContext.Set(accountListItem.Account.Id);

        await Shell.Current.GoToAsync("AccountEditPage");
    }

}