using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PlanAhead.Core.Constants;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Messaging;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Sync;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.Views;
using PlanAhead.Views.Startup;
using Supabase;
using System.ComponentModel;
using System.Diagnostics;

namespace PlanAhead.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    public bool IsSyncing => _syncStatusService.IsSyncing;

    [ObservableProperty]
    private string title = AppConstants.ApplicationName;

    [ObservableProperty]
    private string welcomeMessage =
        "Welcome to Plan Ahead";

    [ObservableProperty]
    private string version =
        $"Version {AppConstants.Version}";

    private readonly ISyncStatusService _syncStatusService;

    private readonly AccountRepository _repository;
    private readonly IApplicationSettingsService _settings;
    private readonly Client _client;
    private readonly IAuthenticationService _authenticationService;
    private readonly IApplicationStartupService _startupService;
    private readonly ISyncService _syncService;
    private readonly ISyncStateService _syncStateService;


    public DashboardViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs, 
        Client client,
        IAuthenticationService authenticationService,
        IApplicationStartupService startupService,
        IApplicationSettingsService settings, 
        ISyncService syncService,
        ISyncStatusService syncStatusService, 
        ISyncStateService syncStateService,
        ILogService logService)
        : base(navigation, dialogs, logService)
    {
        _repository = repository;
        _settings = settings;
        _client = client;
        _authenticationService  = authenticationService;
     
        _startupService = startupService;
        _syncService = syncService;
        _syncStatusService = syncStatusService;
        _syncStateService = syncStateService;

        _syncStatusService.PropertyChanged += SyncStatusChanged;
    }

    public bool IsSyncButtonEnabled => !_syncStatusService.IsSyncing;

    private void SyncStatusChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SyncStatusService.IsSyncing))
        {
            OnPropertyChanged(nameof(IsSyncButtonEnabled));
        }
    }
    [RelayCommand(CanExecute = nameof(CanSync))]
    private async Task SyncAsync()
    {
        try
        {
            await LogService.LogAsync("Manual Sync starts");
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
                        await _syncStateService.UpdateRemoteSyncVersionAsync(userId);
                    }
                    else
                    {
                        await LogService.LogAsync("No changes detected");
                    }
                }
                else
                {
                    await LogService.LogAsync("Could not parse userId");
                }

            }
            else
            {
                await LogService.LogAsync("_authenticationService.GetCurrentUserIdAsync did not return a userIdString");
            }
            await LogService.LogAsync("Manual Sync end");
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
        }
    }


    public string SyncIcon = "sync_auto.png";


    private bool CanSync()
    {
        return !_syncStatusService.IsSyncing;
    }


    [RelayCommand]
    private async Task TestRepository()
    {
        try
        {
            var accounts = await _repository.GetAllAsync();

            await LogService.LogAsync(
                $"Number of accounts = {accounts.Count}");
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
        }
    }


    public async Task RefreshAsync()
    {
        try
        {
            WeakReferenceMessenger.Default.Send(new SyncStatusChangedMessage());
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
        }
    }
}