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
            var msg = $"Error in AccountsViewModel:LoadAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }


    [RelayCommand]
    private async Task DeleteAsync(AccountListItem accountListItem)
    {
        try
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
        catch (Exception ex)
        {
            var msg = $"Error in AccountsViewModel:DeleteAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task AddAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("AccountEditPage");
        }
        catch (Exception ex)
        {
            var msg = $"Error in AccountsViewModel:AddAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task OpenAsync(AccountListItem accountListItem)
    {
        try
        {
            _navigationContext.Set(accountListItem.Account.Id);

            await Shell.Current.GoToAsync("AccountViewPage");
        }
        catch (Exception ex)
        {
            var msg = $"Error in AccountsViewModel:OpenAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task EditAsync(AccountListItem accountListItem)
    {
        try
        {
            _navigationContext.Set(accountListItem.Account.Id);

            await Shell.Current.GoToAsync("AccountEditPage");
        }
        catch (Exception ex)
        {
            var msg = $"Error in AccountsViewModel:EditAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

}