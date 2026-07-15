using HomeBudget.ViewModels;

namespace HomeBudget.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}