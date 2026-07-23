using PlanAhead.ViewModels;

namespace PlanAhead.Views;

public partial class DeveloperToolsPage : ContentPage
{
    public DeveloperToolsPage(
        DeveloperToolsViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}