using PlanAhead.Core.Interfaces.Services;

namespace PlanAhead.Views.Startup;

public partial class SplashPage : ContentPage
{
    private readonly IApplicationStartupService _startup;
    private bool _hasNavigated;

    public SplashPage(IApplicationStartupService startup)
    {
        InitializeComponent();
        _startup = startup;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasNavigated)
            return;

        _hasNavigated = true;

        // IMPORTANT:
        // Yield back to the UI thread so that Shell completes its initial
        // navigation to the Splash page before we perform our startup navigation.
        // Without this, Shell can throw "Pending Navigations still processing".
        await Task.Yield();

        await _startup.NavigateToStartupPageAsync();
    }
}