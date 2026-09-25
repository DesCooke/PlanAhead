using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Logging;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Interfaces;
using PlanAhead.Navigation;
using PlanAhead.Services;
using PlanAhead.ViewModels.Accounts;
using PlanAhead.Views;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;
using PlanAhead.Views.Startup;
using System.Collections.ObjectModel;

namespace PlanAhead.ViewModels.Funds;

public partial class WelcomeViewModel : BaseViewModel
{
    private readonly IApplicationSettingsService _settings;
    private readonly ILocalDatabaseService _localDatabaseService;

    public WelcomeViewModel(
        IApplicationSettingsService settings,
        ILocalDatabaseService localDatabaseService,
        INavigationService navigation,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _settings = settings;
        _localDatabaseService = localDatabaseService;
    }

    [RelayCommand]
    private async Task OfflineAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            _settings.SyncMode = SyncMode.Offline;
            _settings.IsFirstRun = false;
            _settings.LastLocalSyncUtc = new DateTime(2000, 1, 1);
            _settings.LastRemoteSyncUtc = new DateTime(2000, 1, 1);
            _settings.LastLocalUtc = new DateTime(2000, 1, 1);
            await _localDatabaseService.DeleteDatabaseAsync();
            await _localDatabaseService.CreateDatabaseAsync();

            await Shell.Current.GoToAsync("//Dashboard");
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            await Dialogs.ShowExceptionAsync(ex);
        }
    }

    [RelayCommand]
    private async Task OnlineManualAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            _settings.SyncMode = SyncMode.SupabaseManual;
            _settings.IsFirstRun = false;
            _settings.LastLocalSyncUtc = new DateTime(2000, 1, 1);
            _settings.LastRemoteSyncUtc = new DateTime(2000, 1, 1);
            _settings.LastLocalUtc = new DateTime(2000, 1, 1);
            await _localDatabaseService.DeleteDatabaseAsync();
            await _localDatabaseService.CreateDatabaseAsync();

            await Shell.Current.GoToAsync("//Login");
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            await Dialogs.ShowExceptionAsync(ex);
        }
    }

    [RelayCommand]
    private async Task OnlineAutoAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            _settings.SyncMode = SyncMode.SupabaseAuto;
            _settings.IsFirstRun = false;
            _settings.LastLocalSyncUtc = new DateTime(2000, 1, 1);
            _settings.LastRemoteSyncUtc = new DateTime(2000, 1, 1);
            _settings.LastLocalUtc = new DateTime(2000, 1, 1);
            await _localDatabaseService.DeleteDatabaseAsync();
            await _localDatabaseService.CreateDatabaseAsync();

            await Shell.Current.GoToAsync("//Login");
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            await Dialogs.ShowExceptionAsync(ex);
        }
    }
}