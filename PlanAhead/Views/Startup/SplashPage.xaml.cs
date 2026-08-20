using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.DB.SQLite;

namespace PlanAhead.Views.Startup;

public partial class SplashPage : ContentPage
{
    private readonly IApplicationStartupService _startup;
    private bool _hasNavigated;
    private readonly ILocalDatabaseService _localDatabaseService;

    public SplashPage(IApplicationStartupService startup, ILocalDatabaseService localDatabaseService)
    {
        InitializeComponent();
        _startup = startup;
        _localDatabaseService = localDatabaseService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasNavigated)
            return;

        _hasNavigated = true;

        await _localDatabaseService.CreateDatabaseAsync();


        // IMPORTANT:
        // Yield back to the UI thread so that Shell completes its initial
        // navigation to the Splash page before we perform our startup navigation.
        // Without this, Shell can throw "Pending Navigations still processing".
        await Task.Yield();

        await _startup.NavigateToStartupPageAsync();
    }
}