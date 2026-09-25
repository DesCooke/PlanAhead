using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.Logging;
using PlanAhead.Interfaces;
using System.Diagnostics;

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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_hasNavigated)
            return;

        _hasNavigated = true;

        _ = StartAsync();
    }

    private async Task StartAsync()
    {
        await _localDatabaseService.CreateDatabaseAsync();

        await _startup.NavigateToStartupPageAsync();
    }
}