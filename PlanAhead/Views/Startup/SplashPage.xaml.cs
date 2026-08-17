using PlanAhead.Core.Interfaces.Services;

namespace PlanAhead.Views.Startup;

public partial class SplashPage : ContentPage
{
    private readonly IApplicationStartupService _startup;

    public SplashPage(IApplicationStartupService startup)
    {
        InitializeComponent();
        _startup = startup;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _startup.NavigateToStartupPageAsync();
    }
}