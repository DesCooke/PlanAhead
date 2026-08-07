using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Interfaces;
using PlanAhead.Core.Models.Domain;

namespace PlanAhead.ViewModels.Accounts;

public partial class AccountEditViewModel : BaseViewModel
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
    private bool archived = false;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private string iconId = string.Empty;

    public AccountEditViewModel(
        IAccountService accountService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _accountService = accountService;
        _navigationContext = navigationContext;
    }

    public async Task InitialiseAsync()
    {
        if (!_navigationContext.Has<Guid>())
        {
            //
            // New Account
            //
            Title = "New Account";

            return;
        }
        
        Id = _navigationContext.Get<Guid>();

        var account = await _accountService.GetByIdAsync(Id);
        if (account == null)
        {
            //
            // New Account
            //
            Title = "New Account";

            return;
        }

        Title = "Change Account";

        //
        // Existing Account
        //

        Load(account);

        _navigationContext.Clear();
    }

    private void Load(Account account)
    {
        Id = account.Id;
        Name = account.Name;
        Description = account.Description;
        OpeningBalance = account.OpeningBalance;
        Archived = account.Archived;
        Notes = account.Notes;
        IconId = account.IconId;
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
    private Task CancelAsync()
    {
        return Navigation.GoBackAsync();
    }
}