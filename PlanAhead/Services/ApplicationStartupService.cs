using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Views.Startup;

namespace PlanAhead.Services;

public class ApplicationStartupService : IApplicationStartupService
{
    private readonly IApplicationSettingsService _settings;
    private readonly WelcomePage _welcomePage;
    private readonly AppShell _appShell;

    public ApplicationStartupService(
        IApplicationSettingsService settings,
        WelcomePage welcomePage,
        AppShell appShell)
    {
        _settings = settings;
        _welcomePage = welcomePage;
        _appShell = appShell;
    }

    public Task<Page> GetStartupPageAsync()
    {
        if (_settings.IsFirstRun)
            return Task.FromResult<Page>(_welcomePage);

        return Task.FromResult<Page>(_appShell);
    }
}