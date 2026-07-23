using PlanAhead.ViewModels.Funds;

namespace PlanAhead.Views.Funds;

public partial class FundsPage : ContentPage
{
    private readonly FundsViewModel _viewModel;

    public FundsPage(FundsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}