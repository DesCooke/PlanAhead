using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Interfaces;
using PlanAhead.ViewModels.Funds;

namespace PlanAhead.Views.Startup;

public partial class WelcomePage : ContentPage
{
    private WelcomeViewModel _viewModel;

    public WelcomePage(WelcomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

}