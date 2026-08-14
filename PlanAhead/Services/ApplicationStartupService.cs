using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Views.Startup;
using Supabase;
using System.Text.Json;

namespace PlanAhead.Services;

public class ApplicationStartupService : IApplicationStartupService
{
    private readonly IApplicationSettingsService _settings;
    private readonly WelcomePage _welcomePage;
    private readonly LoginPage _loginPage;
    private readonly AppShell _appShell;
    private readonly Client _client;
    private readonly IAuthenticationService _authenticationService;

    public ApplicationStartupService(
        IApplicationSettingsService settings,
        WelcomePage welcomePage,
        LoginPage loginPage,
        Client client,
        IAuthenticationService  authenticationService,
        AppShell appShell)
    {
        _settings = settings;
        _welcomePage = welcomePage;
        _appShell = appShell;
        _loginPage = loginPage;
        _client = client;
        _authenticationService = authenticationService; 
    }

    public async Task<Page> GetStartupPageAsync()
    {
        // first run - go to welcome page
        if (_settings.IsFirstRun)
            return await Task.FromResult<Page>(_welcomePage);

        // if offline only - start the app
        if(_settings.SyncMode==Core.Models.Enums.SyncMode.Offline)
            return await Task.FromResult<Page>(_appShell);

        var json = await SecureStorage.Default.GetAsync("supabase-session");

        if (!string.IsNullOrEmpty(json))
        {
            var session = JsonSerializer.Deserialize<Supabase.Gotrue.Session>(json);

            await _client.Auth.SetSession(
                session.AccessToken,
                session.RefreshToken);
        }

        if (await _authenticationService.IsLoggedInAsync())
            return await Task.FromResult<Page>(_appShell);


        // go to login page
        return await Task.FromResult<Page>(_loginPage);

    }
}