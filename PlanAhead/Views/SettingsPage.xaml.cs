using PlanAhead.ViewModels;

namespace PlanAhead.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(
        SettingsViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}