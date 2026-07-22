using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Interfaces;
using System.Collections.ObjectModel;

namespace PlanAhead.ViewModels.Funds;

public partial class FundListViewModel : BaseViewModel
{
    private readonly IFundService _fundService;

    [ObservableProperty]
    private Fund? selectedFund;


    public ObservableCollection<Fund> Funds { get; }
        = new();



    public FundListViewModel(
        IFundService fundService,
        INavigationService navigation,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _fundService = fundService;

        Title = "Funds";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        await ExecuteBusyAsync(async () =>
        {
            Funds.Clear();

            var accounts =
                await _accountService.GetAllAsync();

            var account =
                accounts.FirstOrDefault();

            if (account == null)
                return;

            var funds =
                await _fundService.GetByAccountIdAsync(account.Id);

            foreach (var fund in funds)
                Funds.Add(fund);
        });
    }

    [RelayCommand]
    private async Task EditAsync(
        Fund fund)
    {
    }

    [RelayCommand]
    private async Task DeleteAsync(
        Fund fund)
    {
    }
}