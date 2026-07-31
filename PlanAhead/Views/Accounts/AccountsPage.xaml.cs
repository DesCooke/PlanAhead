using PlanAhead.ViewModels.Accounts;
using PlanAhead.ViewModels.Funds;

namespace PlanAhead.Views.Accounts;

public partial class AccountsPage : ContentPage
{
    private readonly AccountsViewModel _viewModel;

    public AccountsPage(AccountsViewModel viewModel)
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