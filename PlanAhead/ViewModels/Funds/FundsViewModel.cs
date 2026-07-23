using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Interfaces;
using PlanAhead.Views.Funds;
using System.Collections.ObjectModel;

namespace PlanAhead.ViewModels.Funds;

public partial class FundsViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly IFundService _fundService;

    public ObservableCollection<Fund> Funds { get; } = new();

    public FundsViewModel(
        INavigationService navigation,
        IDialogService dialogs,
        IAccountService accountService,
        IFundService fundService)
        : base(navigation, dialogs)
    {
        Title = "Funds";
        _accountService = accountService;
        _fundService = fundService;
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
        });
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Navigation.NavigateToAsync<FundEditPage>();
    }
}