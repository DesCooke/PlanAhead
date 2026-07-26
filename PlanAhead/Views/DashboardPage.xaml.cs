using PlanAhead.Resources.Icons;
using PlanAhead.ViewModels;

namespace PlanAhead.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}