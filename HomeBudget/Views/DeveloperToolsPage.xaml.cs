using HomeBudget.ViewModels;

namespace HomeBudget.Views;

public partial class DeveloperToolsPage : ContentPage
{
    public DeveloperToolsPage(
        DeveloperToolsViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}