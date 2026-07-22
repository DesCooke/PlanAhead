using PlanAhead.ViewModels.Funds;

namespace PlanAhead.Views;

public partial class FundListPage : ContentPage
{
    public FundListPage(
        FundListViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm =
            (FundListViewModel)BindingContext;

        await vm.LoadCommand.ExecuteAsync(null);
    }
}