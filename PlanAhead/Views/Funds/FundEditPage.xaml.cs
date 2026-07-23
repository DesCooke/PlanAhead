using PlanAhead.ViewModels.Funds;

namespace PlanAhead.Views.Funds;

public partial class FundEditPage : ContentPage
{
    private readonly FundEditViewModel _viewModel;

    public FundEditPage(
        FundEditViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitialiseAsync();
    }
}