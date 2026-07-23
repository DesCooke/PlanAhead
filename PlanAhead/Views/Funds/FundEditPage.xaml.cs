using PlanAhead.ViewModels.Funds;

namespace PlanAhead.Views.Funds;

public partial class FundEditPage : ContentPage
{
    public FundEditPage(
        FundEditViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}