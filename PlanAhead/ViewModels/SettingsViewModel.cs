using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.DB.Supabase;
using PlanAhead.Infrastructure.Logging;
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
            IAutoSyncService autoSyncService,
            ILogService logService)
        : base(navigation, dialogs, logService)
        {
            _authenticationService = authenticationService;
            _localDatabase = localDatabase;
            _remoteDatabase = remoteDatabase;
            _applicationSettingsService = applicationSettingsService;
            _autoSyncService = autoSyncService;
        }


        [RelayCommand(FlowExceptionsToTaskScheduler = true)]
        private async Task DeveloperTools()
        {
            await Shell.Current.GoToAsync("DeveloperToolsPage");
        }



        [RelayCommand(FlowExceptionsToTaskScheduler = true)]
        private async Task LogoutAsync()
        {
            if (!await DialogService.ConfirmAsync(
                    "Logout",
                    "Are you sure you want to logout?"))
                return;

            await _authenticationService.LogoutAsync();

            await _autoSyncService.StopAsync();

            SecureStorage.Default.Remove("supabase-session");

            await Shell.Current.GoToAsync("//Login");
        }

        [RelayCommand(FlowExceptionsToTaskScheduler = true)]
        private async Task ClearLocalDatabase()
        {
            if (!await DialogService.ConfirmAsync(
                    "Clear Local Database",
                    "Are you sure you want to clear the local database?.  This cannot be undone"))
                return;

            await _localDatabase.DeleteDatabaseAsync();

            _applicationSettingsService.ResetToFactory();

            await Shell.Current.GoToAsync("//Dashboard");
        }

        [RelayCommand(FlowExceptionsToTaskScheduler = true)]
        private async Task Diagnostics()
        {
            await Shell.Current.GoToAsync("DiagnosticsPage");
        }

        [RelayCommand(FlowExceptionsToTaskScheduler = true)]
        private async Task ClearRemoteDatabase()
        {
            if (!await DialogService.ConfirmAsync(
                    "Clear Remote Database",
                    "Are you sure you want to clear the remote database?.  This cannot be undone"))
                return;

            await _remoteDatabase.DeleteUserDataAsync();

            SecureStorage.Default.Remove("supabase-session");

            await Shell.Current.GoToAsync("//Welcome");
        }

        [RelayCommand(FlowExceptionsToTaskScheduler = true)]
        private async Task FactoryReset()
        {
                if (!await DialogService.ConfirmAsync(
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

        [RelayCommand(FlowExceptionsToTaskScheduler = true)]
        private async Task ChangeSignIn()
        {
            await Shell.Current.GoToAsync("//Welcome");
        }

        public async Task InitialiseAsync()
        {
            IsLoggedIn = await _authenticationService.IsLoggedInAsync();
        }

    }
}
