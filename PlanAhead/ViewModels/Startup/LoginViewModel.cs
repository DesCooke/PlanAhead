using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Logging;
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
    private ISyncStateService _syncStateService;
    private ISupabaseClientProvider _provider;
    private ISecureStorageService _secureStorageService;
  


    public LoginViewModel(
        IApplicationSettingsService settings,
        IAuthenticationService authenticationService,
        INavigationService navigation,
        IDialogService dialogs,
        ISyncService syncService,
        IAutoSyncService autoSyncService,
        ISyncStateService syncStateService,
        ILogService logService,
        ISupabaseClientProvider provider,
        ISecureStorageService secureStorageService)
        : base(navigation, dialogs, logService)
    {
        _settings = settings;
        _authenticationService = authenticationService;
        _syncService = syncService;
        _autoSyncService = autoSyncService;
        _syncStateService = syncStateService;
        _provider = provider;
        _secureStorageService = secureStorageService;
    }


    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task LoginAsync()
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
                {
                    bool hasLocalChanges = await _syncStateService.HasLocalChangesAsync();
                    bool hasRemoteChanges = await _syncStateService.HasRemoteChangesAsync(userId);
                    if (hasLocalChanges || hasRemoteChanges)
                    {
                        await _syncService.SyncAsync(userId, hasLocalChanges, hasRemoteChanges);
                    }
                }
                if (_settings.SyncMode == PlanAhead.Core.Models.Enums.SyncMode.SupabaseAuto)
                {
                    _autoSyncService.Start(userId);
                }
            }


            await Shell.Current.GoToAsync("//Dashboard");
    }


    public async Task InitialiseAsync()
    {
        Email = "";
        Password = "";
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task RegisterAsync()
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

            await DialogService.ShowMessageAsync(
                "Registration",
                "Your account has been created and logged in.");

            await Shell.Current.GoToAsync("//Dashboard");
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task OfflineOnlyAsync()
    {
        _settings.SyncMode = PlanAhead.Core.Models.Enums.SyncMode.Offline;

        await Shell.Current.GoToAsync("//Dashboard");
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task GoogleAsync()
    {
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task CancelAsync()
    {
    }
}