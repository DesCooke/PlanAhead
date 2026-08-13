using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Views.Startup;

namespace PlanAhead.Services;

public class ApplicationStartupService : IApplicationStartupService
{
    private readonly IApplicationSettingsService _settings;
    private readonly WelcomePage _welcomePage;
    private readonly LoginPage _loginPage;
    private readonly AppShell _appShell;

    public ApplicationStartupService(
        IApplicationSettingsService settings,
        WelcomePage welcomePage,
        LoginPage loginPage,
        AppShell appShell)
    {
        _settings = settings;
        _welcomePage = welcomePage;
        _appShell = appShell;
        _loginPage = loginPage;
    }

    public Task<Page> GetStartupPageAsync()
    {
        // first run - go to welcome page
        if (_settings.IsFirstRun)
            return Task.FromResult<Page>(_welcomePage);

        // if offline only - start the app
        if(_settings.SyncMode==Core.Models.Enums.SyncMode.Offline)
            return Task.FromResult<Page>(_appShell);

        // go to login page
        return Task.FromResult<Page>(_loginPage);

    }
}