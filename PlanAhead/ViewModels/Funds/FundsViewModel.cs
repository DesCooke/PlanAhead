using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Infrastructure.Sync;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels.Accounts;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;
using System.Collections.ObjectModel;

namespace PlanAhead.ViewModels.Funds;

public partial class FundsViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly IFundService _fundService;
    private readonly INavigationContext _navigationContext;
    private readonly ISyncStateService _syncStateService;

    public ObservableCollection<Fund> Funds { get; } = new();

    public bool HasFunds => Funds.Any();
    public bool HasNoFunds => !HasFunds;

    [ObservableProperty]
    private Guid accountId;

    [ObservableProperty]
    private Fund? selectedFund;

    public FundsViewModel(
        IAccountService accountService,
        IFundService fundService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IDialogService dialogs,
        ISyncStateService syncStateService)
        : base(navigation, dialogs)
    {
        _accountService = accountService;
        _fundService = fundService;
        _navigationContext = navigationContext;
        _syncStateService = syncStateService;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_navigationContext.Has<Guid>())
            {
                AccountId = _navigationContext.Get<Guid>();
                _navigationContext.Clear();
            }

            if (AccountId != Guid.Empty)
            {
                await LoadAsync(AccountId);
            }
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundsViewModel:InitialiseAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task LoadAsync(Guid accountId)
    {
        try
        {
            var funds = await _fundService.GetByAccountIdAsync(accountId);

            Funds.Clear();
            foreach (var fund in funds)
                Funds.Add(fund);

            RefreshUi(
                nameof(HasFunds),
                nameof(HasNoFunds),
                nameof(Title));

            Title = $"Funds ({Funds.Count})";
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundsViewModel:LoadAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task DeleteAsync(Fund fund)
    {
        try
        {
            var delete =
                await Dialogs.ConfirmAsync(
                    "Delete Fund",
                    $"Delete '{fund.Name}'?");

            if (!delete)
                return;

            await _fundService.DeleteAsync(fund);

            await _syncStateService.IncreaseLocalVersion();

            await LoadAsync(AccountId);
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundsViewModel:DeleteAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    [RelayCommand]
    private async Task AddAsync()
    {
        try
        {
            _navigationContext.Set(AccountId);

            await Shell.Current.GoToAsync("FundEditPage");
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundsViewModel:AddAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    partial void OnSelectedFundChanged(Fund? value)
    {
        if (value == null)
            return;

        EditCommand.Execute(value);
    }

    [RelayCommand]
    private async Task OpenASync(Fund fund)
    {
        try
        {
            _navigationContext.Set(fund.Id);

            await Shell.Current.GoToAsync("FundViewPage");
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundsViewModel:OpenAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private async Task EditAsync(Fund fund)
    {
        try
        {
            _navigationContext.Set(fund);

            await Shell.Current.GoToAsync("FundEditPage");

            SelectedFund = null;
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundsViewModel:EditAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

}