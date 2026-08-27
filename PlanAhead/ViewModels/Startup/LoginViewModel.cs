using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Sync;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels;
using PlanAhead.Views;
using PlanAhead.Views.Startup;
using System.Text.Json;

public partial class LoginViewModel : BaseViewModel
{
    private IApplicationSettingsService _settings;
    private IAuthenticationService _authenticationService;
    private ISyncService _syncService;
    private IAutoSyncService _autoSyncService;
    

    public LoginViewModel(
        IApplicationSettingsService settings,
        IAuthenticationService authenticationService,
        INavigationService navigation,
        IDialogService dialogs,
        ISyncService syncService,
        IAutoSyncService autoSyncService)
        : base(navigation, dialogs)
    {
        _settings = settings;
        _authenticationService = authenticationService;
        _syncService = syncService;
        _autoSyncService = autoSyncService;
    }


    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            var response = await _authenticationService.LoginAsync(Email, Password);

            await SecureStorage.Default.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(response));

            var userIdString = await _authenticationService.GetCurrentUserIdAsync();
            if (userIdString != null)
            {
                var userId = Guid.Parse(userIdString);
                if (userId != Guid.Empty)
                    await _syncService.SyncAsync(userId);
                if (_settings.SyncMode == PlanAhead.Core.Models.Enums.SyncMode.SupabaseAuto)
                {
                    _autoSyncService.Start(userId);
                }
            }


            await Shell.Current.GoToAsync("//Dashboard");
        }
        catch (Exception ex)
        {
            var msg = $"Error in LoginViewModel:LoginAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }


    public async Task InitialiseAsync()
    {
        try
        {
            Email = "";
            Password = "";
        }
        catch (Exception ex)
        {
            var msg = $"Error in LoginViewModel:InitialiseAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        try
        {
            await _authenticationService.RegisterAsync(
                Email,
                Password);

            var response = await _authenticationService.LoginAsync(Email, Password);

            if (_settings.SyncMode == PlanAhead.Core.Models.Enums.SyncMode.SupabaseAuto)
            {
                var userIdStr = await _authenticationService.GetCurrentUserIdAsync();
                if(userIdStr != null)
                {
                    var userId = Guid.Parse(userIdStr);
                    _autoSyncService.Start(userId);
                }
            }

            await SecureStorage.Default.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(response));

            await Dialogs.ShowMessageAsync(
                "Registration",
                "Your account has been created and logged in.");

            await Shell.Current.GoToAsync("//Dashboard");
        }
        catch (Exception ex)
        {
            var msg = $"Error in LoginViewModel:RegisterAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private async Task OfflineOnlyAsync()
    {
        try
        {
            _settings.SyncMode = PlanAhead.Core.Models.Enums.SyncMode.Offline;

            await Shell.Current.GoToAsync("//Dashboard");
        }
        catch (Exception ex)
        {
            var msg = $"Error in LoginViewModel:OfflineOnlyAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private async Task GoogleAsync()
    {
        try
        {

        }
        catch (Exception ex)
        {
            var msg = $"Error in LoginViewModel:GoogleAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        try
        {

        }
        catch (Exception ex)
        {
            var msg = $"Error in LoginViewModel:CancelAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }
}