using CommunityToolkit.Maui.Extensions;
using PlanAhead.ViewModels.Funds;
using PlanAhead.Views.Popups;

namespace PlanAhead.Views.Funds;

public partial class FundEditPage : ContentPage
{
    private readonly FundEditViewModel _viewModel;

    public FundEditPage(
        FundEditViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = viewModel;
    }

    private async void ChooseIcon_Clicked(object sender, EventArgs e)
    {
        try
        {
            var popup = Handler!.MauiContext!
                .Services
                .GetRequiredService<IconPickerPopup>();

            await this.ShowPopupAsync(popup);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Exception", ex.ToString(), "OK");
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitialiseAsync();
    }
}