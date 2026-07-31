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

    public ObservableCollection<Account> Accounts{ get; } = new();

    public bool HasAccounts => Accounts.Any();
    public bool HasNoAccounts => !HasAccounts;


    [ObservableProperty]
    private Account? selectedAccount;

    public AccountsViewModel(
        IAccountService accountService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _accountService = accountService;
        _navigationContext = navigationContext;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        await ExecuteBusyAsync(async () =>
        {
            var accounts = await _accountService.GetAllAsync();

            Accounts.Clear();
            foreach (var account in accounts)
                Accounts.Add(account);

            RefreshUi(
                nameof(HasAccounts),
                nameof(HasNoAccounts),
                nameof(Title));
        });

        Title = $"Accounts ({Accounts.Count})";
    }

    [RelayCommand]
    private async Task DeleteAsync(Account account)
    {
        var delete =
            await Dialogs.ConfirmAsync(
                "Delete Account",
                $"Delete '{account.Name}'?");

        if (!delete)
            return;

        await _accountService.DeleteAsync(account.Id);

        await LoadAsync();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Navigation.NavigateToAsync<AccountEditPage>();
    }

    partial void OnSelectedAccountChanged(Account? value)
    {
        if (value == null)
            return;

        EditCommand.Execute(value);
    }

    [RelayCommand]
    private async Task EditAsync(Account account)
    {
        _navigationContext.Set(account);

        await Navigation.NavigateToAsync<AccountEditPage>();

        SelectedAccount = null;
    }

}