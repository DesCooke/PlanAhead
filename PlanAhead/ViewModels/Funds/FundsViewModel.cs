using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels.Accounts;
using PlanAhead.Views.Funds;
using System.Collections.ObjectModel;

namespace PlanAhead.ViewModels.Funds;

public partial class FundsViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly IFundService _fundService;
    private readonly INavigationContext _navigationContext;

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
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _accountService = accountService;
        _fundService = fundService;
        _navigationContext = navigationContext;
    }

    public async Task InitialiseAsync()
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
            await Dialogs.ShowErrorAsync("FundsViewModal.cs:LoadAsync:Exception:" + ex.ToString());
        }

    }

    [RelayCommand]
    private async Task DeleteAsync(Fund fund)
    {
        var delete =
            await Dialogs.ConfirmAsync(
                "Delete Fund",
                $"Delete '{fund.Name}'?");

        if (!delete)
            return;

        await _fundService.DeleteAsync(fund.Id);

        await LoadAsync(AccountId);
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        _navigationContext.Set(AccountId);
        await Navigation.NavigateToAsync<FundEditPage>();
    }

    partial void OnSelectedFundChanged(Fund? value)
    {
        if (value == null)
            return;

        EditCommand.Execute(value);
    }

    [RelayCommand]
    private async Task EditAsync(Fund fund)
    {
        _navigationContext.Set(fund);

        await Navigation.NavigateToAsync<FundEditPage>();

        SelectedFund = null;
    }

}