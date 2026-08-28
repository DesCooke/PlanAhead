using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Infrastructure.Sync;
using PlanAhead.Interfaces;
using PlanAhead.Services;

namespace PlanAhead.ViewModels.Accounts;

public partial class AccountEditViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly INavigationContext _navigationContext;
    private readonly IDialogService _dialogService;
    private readonly ISyncStateService _syncStateService;

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
        IDialogService dialogService,
        IDialogService dialogs,
        ISyncStateService syncStateService)
        : base(navigation, dialogs)
    {
        _accountService = accountService;
        _navigationContext = navigationContext;
        _dialogService = dialogService;
        _syncStateService = syncStateService;
    }

    public async Task InitialiseAsync()
    {
        try
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
        catch (Exception ex)
        {
            var msg = $"Error in AccountEditViewModel:InitialiseAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

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
        try
        {
            var error = Validate();
            if (error != null)
            {
                await Dialogs.ShowMessageAsync(
                    "Validation",
                    error);

                return;
            }

            if (Id == Guid.Empty)
                await _accountService.AddAsync(Build());
            else
                await _accountService.UpdateAsync(Build());

            await _syncStateService.IncreaseLocalVersion();

            await Navigation.GoBackAsync();
        }
        catch (Exception ex)
        {
            var msg = $"Error in AccountEditViewModel:SaveAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        try
        {
            return Navigation.GoBackAsync();
        }
        catch (Exception ex)
        {
            var msg = $"Error in AccountEditViewModel:CancelAsync:{ex.Message}";
            Dialogs.ShowErrorAsync(msg);
        }
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task ChooseIconAsync()
    {
        try
        {
            var iconId = await _dialogService.PickIconAsync(IconId);

            if (iconId != null)
                IconId = iconId;
        }
        catch (Exception ex)
        {
            var msg = $"Error in AccountEditViewModel:ChooseIconAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }
}