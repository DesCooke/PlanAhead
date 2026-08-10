using CommunityToolkit.Maui.Extensions;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels.Accounts;
using PlanAhead.Views.Popups;

namespace PlanAhead.Views.Accounts;

public partial class AccountEditPage : ContentPage
{
    private readonly AccountEditViewModel _viewModel;
    private readonly IDialogService _dialogService;

    public AccountEditPage(
        AccountEditViewModel viewModel,
        IDialogService dialogService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _dialogService = dialogService;

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitialiseAsync();
    }

}