using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Interfaces;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;

namespace PlanAhead.ViewModels.Accounts;

public partial class AccountViewViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly INavigationContext _navigationContext;

    public IEnumerable<Frequency> Frequencies =>
        Enum.GetValues<Frequency>();


    [ObservableProperty]
    private Guid id;

    [ObservableProperty]
    private Guid accountId;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private Decimal openingBalance = 0.00M;

    [ObservableProperty]
    private string openingBalanceDisplay = string.Empty;

    [ObservableProperty]
    private bool archived = false;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private string iconId = string.Empty;

    [ObservableProperty]
    public Status health = Status.Green;

    public AccountViewViewModel(
        IAccountService accountService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _accountService = accountService;
        _navigationContext = navigationContext;

        Title = "Account Details";
    }

    private void Load(Account account)
    {
        Id = account.Id;
        Name = account.Name;
        Description = account.Description;
        OpeningBalance = account.OpeningBalance;
        OpeningBalanceDisplay = account.OpeningBalanceDisplay;
        Archived = account.Archived;
        Notes = account.Notes;
        IconId = account.IconId;


        Health =
                OpeningBalance switch
                {
                    < 0 => Status.Red,
                    < 500 => Status.Amber,
                    _ => Status.Green
                };

    }

    private Account Build()
    {
        return new Account
        {
            Id = Id,
            Name = Name.Trim(),
            Description = Description.Trim(),
            OpeningBalance = OpeningBalance,
            Archived = Archived,
            Notes = Notes.Trim(),
            IconId = IconId.Trim()
        };
    }

    string? Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return ("Please enter a account name.");

        return null;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var error = Validate();
        if (error != null)
        {
            await Dialogs.ShowMessageAsync(
                "Validation",
                error);

            return;
        }

        try
        {
            if (Id == Guid.Empty)
                await _accountService.AddAsync(Build());
            else
                await _accountService.UpdateAsync(Build());

            await Navigation.GoBackAsync();
        }
        catch (Exception ex)
        {
            await Dialogs.ShowErrorAsync(ex.Message);
        }
    
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if(Id == Guid.Empty){
            Id = _navigationContext.Get<Guid>();
            _navigationContext.Clear();
        }

        var account = await _accountService.GetByIdAsync(Id);

        if (account != null)
            Load(account);
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        _navigationContext.Set(Id);

        await Navigation.NavigateToAsync<AccountEditPage>();
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        return Navigation.GoBackAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        var delete =
            await Dialogs.ConfirmAsync(
                "Delete Account",
                $"Delete '{Name}'?");

        if (!delete)
            return;

        await _accountService.DeleteAsync(Id);

        await Navigation.GoBackAsync();
    }

    [RelayCommand]
    private async Task GoToPlansAsync()
    {
        var hey =
            await Dialogs.ConfirmAsync(
                "Hello World",
                $"Hello");
    }

    [RelayCommand]
    private async Task GoToFundsAsync()
    {
        try
        {
            _navigationContext.Set(Id);

            await Navigation.NavigateToAsync<FundsPage>();
        }
        catch (Exception ex)
        {
            await Dialogs.ShowErrorAsync("AccountViewViewModel.cs:GoToFundsAsync:Exception:" + ex.ToString());
        }
    }

}