using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Views.Startup;
using Supabase;
using System.Text.Json;

namespace PlanAhead.Services;

public class ApplicationStartupService : IApplicationStartupService
{
    private readonly IApplicationSettingsService _settings;
    private readonly Client _client;
    private readonly IAuthenticationService _authenticationService;

    public ApplicationStartupService(
        IApplicationSettingsService settings,
        Client client,
        IAuthenticationService  authenticationService)
    {
        _settings = settings;
        _client = client;
        _authenticationService = authenticationService; 
    }

    public async Task NavigateToStartupPageAsync()
    {
        // first run - go to welcome page
        if (_settings.IsFirstRun)
        {
            await Shell.Current.GoToAsync("//Welcome");
            return;
        }

        // if offline only - start the app
        if (_settings.SyncMode == Core.Models.Enums.SyncMode.Offline)
        {
            await Shell.Current.GoToAsync("//Dashboard");
            return;
        }

        var json = await SecureStorage.Default.GetAsync("supabase-session");

        if (!string.IsNullOrEmpty(json))
        {
            var session = JsonSerializer.Deserialize<Supabase.Gotrue.Session>(json);

            await _client.Auth.SetSession(
                session.AccessToken,
                session.RefreshToken);
        }

        if (await _authenticationService.IsLoggedInAsync())
        {
            await Shell.Current.GoToAsync("//Dashboard");
            return;
        }


        // go to login page
        await Shell.Current.GoToAsync("//Login");

    }
}