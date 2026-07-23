using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Interfaces;
using PlanAhead.Views.Funds;
using PlanAhead.Services;
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

        Title = "Funds";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        await ExecuteBusyAsync(async () =>
        {
            Funds.Clear();
            /*
                        var accounts = await _accountService.GetAllAsync();
                        var account = accounts.FirstOrDefault();

                        if (account == null)
                            return;

                        var funds = await _fundService.GetByAccountIdAsync(account.Id);
            */
            var funds = await _fundService.GetAllAsync();

            foreach (var fund in funds)
                Funds.Add(fund);

            OnPropertyChanged(nameof(HasFunds));
            OnPropertyChanged(nameof(HasNoFunds));
        });
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

        await LoadAsync();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
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