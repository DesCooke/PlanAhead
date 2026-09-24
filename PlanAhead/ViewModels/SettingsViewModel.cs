using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Logging;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.DB.Supabase;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Services;
using PlanAhead.Infrastructure.Sync;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalDatabaseService _localDatabase;
        private readonly IRemoteDatabaseService _remoteDatabase;
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IAutoSyncService _autoSyncService;


        [ObservableProperty]
        private bool isLoggedIn = false;

        public SettingsViewModel(INavigationService navigation,
            IDialogService dialogs, 
            IAuthenticationService authenticationService,
            ILocalDatabaseService localDatabase,
            IRemoteDatabaseService remoteDatabase, 
            IApplicationSettingsService applicationSettingsService,
            IAutoSyncService autoSyncService) : base(navigation, dialogs)
        {
            _authenticationService = authenticationService;
            _localDatabase = localDatabase;
            _remoteDatabase = remoteDatabase;
            _applicationSettingsService = applicationSettingsService;
            _autoSyncService = autoSyncService;
        }


        [RelayCommand]
        private async Task DeveloperTools()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                await Shell.Current.GoToAsync("DeveloperToolsPage");
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }



        [RelayCommand]
        private async Task LogoutAsync()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                if (!await Dialogs.ConfirmAsync(
                        "Logout",
                        "Are you sure you want to logout?"))
                    return;

                await _authenticationService.LogoutAsync();

                await _autoSyncService.StopAsync();

                SecureStorage.Default.Remove("supabase-session");

                await Shell.Current.GoToAsync("//Login");
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }

        [RelayCommand]
        private async Task ClearLocalDatabase()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                if (!await Dialogs.ConfirmAsync(
                        "Clear Local Database",
                        "Are you sure you want to clear the local database?.  This cannot be undone"))
                    return;

                await _localDatabase.DeleteDatabaseAsync();

                _applicationSettingsService.ResetToFactory();

                await Shell.Current.GoToAsync("//Welcome");
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }

        [RelayCommand]
        private async Task Diagnostics()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                await Shell.Current.GoToAsync("DiagnosticsPage");
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }

        [RelayCommand]
        private async Task ClearRemoteDatabase()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                if (!await Dialogs.ConfirmAsync(
                        "Clear Remote Database",
                        "Are you sure you want to clear the remote database?.  This cannot be undone"))
                    return;

                await _remoteDatabase.DeleteUserDataAsync();

                SecureStorage.Default.Remove("supabase-session");

                await Shell.Current.GoToAsync("//Welcome");
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }

        [RelayCommand]
        private async Task FactoryReset()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                if (!await Dialogs.ConfirmAsync(
                        "Factory Reset?",
                        "This will clear the local and remote database and clear settings.  This cannot be undone"))
                    return;

                await _authenticationService.EnsureSessionAsync();

                await _remoteDatabase.DeleteUserDataAsync();

                await _localDatabase.DeleteDatabaseAsync();

                _applicationSettingsService.ResetToFactory();

                SecureStorage.Default.Remove("supabase-session");

                await Shell.Current.GoToAsync("//Welcome");
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }

        [RelayCommand]
        private async Task ChangeSignIn()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                await Shell.Current.GoToAsync("//Welcome");
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }

        public async Task InitialiseAsync()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                IsLoggedIn = await _authenticationService.IsLoggedInAsync();
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                await Dialogs.ShowExceptionAsync(ex);
            }
        }

    }
}
