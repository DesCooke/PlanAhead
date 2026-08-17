using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Core.Messaging;

namespace PlanAhead.Controls;

public partial class SyncStatusBarViewModel : ObservableRecipient, IRecipient<SyncStatusChangedMessage>
{
    private readonly IApplicationSettingsService _settings;
    private readonly IAuthenticationService _authentication;
    private readonly IConnectivityService _connectivity;

    [ObservableProperty]
    private string userEmail = string.Empty;

    [ObservableProperty]
    private string connectionStatus = "Checking...";

    [ObservableProperty]
    private string syncText = "";

    [ObservableProperty]
    private string syncIcon = "";

    [ObservableProperty]
    private bool isOnline = false;


    public SyncStatusBarViewModel(
        IApplicationSettingsService settings,
        IAuthenticationService authentication,
        IConnectivityService connectivityService)
    {
        _settings = settings;
        _authentication = authentication;
        _connectivity = connectivityService;

        IsActive = true;

        _ = RefreshAsync();
    }

    public async void Receive(SyncStatusChangedMessage message)
    {
        await RefreshAsync();
    }

    public async Task RefreshAsync()
    {
        if (!_connectivity.IsOnline) {
            SyncText = "YOU ARE OFFLINE";
            IsOnline = false;
            SyncIcon = "sync_disabled.png";
            UserEmail = string.Empty;
            ConnectionStatus = "";
            return;
        }

        //
        // User has connection to the internet - so
        // display correct connection status
        //
        SyncText = _settings.SyncMode switch
        {
            SyncMode.Offline => "Offline",
            SyncMode.SupabaseManual => "Manual Sync",
            SyncMode.SupabaseAuto => "Auto Sync",
            _ => "Unknown"
        };

        IsOnline =
                _settings.SyncMode != SyncMode.Offline;

        SyncIcon = _settings.SyncMode switch
        {
            SyncMode.Offline => "sync_disabled.png",
            SyncMode.SupabaseManual => "sync_manual.png",
            SyncMode.SupabaseAuto => "sync_auto.png",
            _ => ""
        };



        if (!IsOnline)
        {
            UserEmail = string.Empty;
            ConnectionStatus = "Offline";
            return;
        }

        UserEmail = await _authentication.GetCurrentUserEmailAsync()
                     ?? "Not logged in";

        ConnectionStatus =
            await _authentication.IsLoggedInAsync()
                ? "Connected"
                : "Not connected";
    }
}