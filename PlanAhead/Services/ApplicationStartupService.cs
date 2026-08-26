using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Sync;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB;
using PlanAhead.Infrastructure.Sync;
using PlanAhead.Interfaces;
using PlanAhead.Views.Startup;
using Supabase;
using System.Text.Json;

namespace PlanAhead.Services;

public class ApplicationStartupService : IApplicationStartupService
{
    private readonly IApplicationSettingsService _settings;
    private readonly Client _client;
    private readonly IAuthenticationService _authenticationService;
    private readonly IDialogService _dialogs;
    private readonly IConnectivityService _connectivityService;
    private readonly IAutoSyncService _autoSyncService;
    private readonly ISyncStatusService _syncStatusService;

    public ApplicationStartupService(
        IApplicationSettingsService settings,
        Client client,
        IAuthenticationService  authenticationService,
        IDialogService dialogs, 
        IConnectivityService connectivityService,
        IAutoSyncService autoSyncService,
        ISyncStatusService syncStatusService)
    {
        _settings = settings;
        _client = client;
        _authenticationService = authenticationService; 
        _dialogs = dialogs;
        _connectivityService = connectivityService;
        _autoSyncService = autoSyncService;
        _syncStatusService = syncStatusService;
    }

    public async Task NavigateToStartupPageAsync()
    {
        try
        {
            _syncStatusService.IsSyncing = false;

            //
            // User is currently offline - go into offline mode
            //
            if (!_connectivityService.IsOnline)
            {
                await _dialogs.ShowMessageAsync("You are OFFLINE",
                    "You appear to be offline - reading and writing will be restricted to this device only - until internet service resumes");
                await Shell.Current.GoToAsync("//Dashboard");
                return;
            }


            //
            // User is online - first time of running - offer different ways to connect
            //
            if (_settings.IsFirstRun)
            {
                await Shell.Current.GoToAsync("//Welcome");
                return;
            }


            //
            // User is online - not first time of running - use he saved method
            //


            // if offline only - start the app
            if (_settings.SyncMode == Core.Models.Enums.SyncMode.Offline)
            {
                await Shell.Current.GoToAsync("//Dashboard");
                return;
            }

            await _authenticationService.RestoreSessionAsync();

            if (await _authenticationService.IsLoggedInAsync())
            {
                if (_settings.SyncMode == PlanAhead.Core.Models.Enums.SyncMode.SupabaseAuto)
                {
                    var userIdStr = await _authenticationService.GetCurrentUserIdAsync();
                    if (userIdStr != null)
                    {
                        var userId = Guid.Parse(userIdStr);
//                        _autoSyncService.Start(userId);
                    }
                }

                await Shell.Current.GoToAsync("//Dashboard");
                return;
            }


            // go to login page
            await Shell.Current.GoToAsync("//Login");

        }
        catch (Exception ex)
        {
            await _dialogs.ShowErrorAsync(
                $"Unable to navigate to startup page.  {ex.Message}");
        }

    }
}