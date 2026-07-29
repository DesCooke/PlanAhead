using CommunityToolkit.Maui.Extensions;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels.Funds;
using PlanAhead.Views.Popups;

namespace PlanAhead.Views.Funds;

public partial class FundEditPage : ContentPage
{
    private readonly FundEditViewModel _viewModel;
    private readonly IDialogService _dialogService;

    public FundEditPage(
        FundEditViewModel viewModel,
        IDialogService dialogService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _dialogService = dialogService;

        BindingContext = viewModel;
    }

    private async void ChooseIcon_Clicked(object sender, EventArgs e)
    {
        var icon = await _dialogService.PickIconAsync(_viewModel.IconId);

        if (icon != null)
        {
            _viewModel.IconId = icon;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitialiseAsync();
    }

    private async void ChooseIcon_Tapped(object sender, TappedEventArgs e)
    {
        var iconId = await _dialogService.PickIconAsync(_viewModel.IconId);

        if (iconId != null)
            _viewModel.IconId = iconId;
    }
}