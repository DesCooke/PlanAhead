using PlanAhead.Interfaces;
using PlanAhead.ViewModels.Funds;

namespace PlanAhead.Views.Funds;

public partial class FundViewPage : ContentPage
{
    private readonly FundViewViewModel _viewModel;
    private readonly IDialogService _dialogService;

    public FundViewPage(
        FundViewViewModel viewModel,
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

        await _viewModel.LoadCommand.ExecuteAsync(null);
    }


    private async void ChooseIcon_Tapped(object sender, TappedEventArgs e)
    {
        var iconId = await _dialogService.PickIconAsync(_viewModel.IconId);

        if (iconId != null)
            _viewModel.IconId = iconId;
    }
}