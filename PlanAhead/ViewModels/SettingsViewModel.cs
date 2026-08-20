using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.DB.Supabase;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Services;
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


        [ObservableProperty]
        private bool isLoggedIn = false;

        public SettingsViewModel(INavigationService navigation,
            IDialogService dialogs, 
            IAuthenticationService authenticationService,
            ILocalDatabaseService localDatabase,
            IRemoteDatabaseService remoteDatabase, 
            IApplicationSettingsService applicationSettingsService) : base(navigation, dialogs)
        {
            _authenticationService = authenticationService;
            _localDatabase = localDatabase;
            _remoteDatabase = remoteDatabase;
            _applicationSettingsService = applicationSettingsService;
        }


        [RelayCommand]
        private async Task DeveloperTools()
        {
            try
            {
                await Shell.Current.GoToAsync("DeveloperToolsPage");
            }
            catch (Exception ex)
            {
                var msg = $"Error in SettingsViewModel:DeveloperTools:{ex.Message}";
                await Dialogs.ShowErrorAsync(msg);
            }

        }



        [RelayCommand]
        private async Task LogoutAsync()
        {
            try
            {
                if (!await Dialogs.ConfirmAsync(
                        "Logout",
                        "Are you sure you want to logout?"))
                    return;

                await _authenticationService.LogoutAsync();

                SecureStorage.Default.Remove("supabase-session");

                await Shell.Current.GoToAsync("//Login");
            }
            catch (Exception ex)
            {
                var msg = $"Error in SettingsViewModel:LogoutAsync:{ex.Message}";
                await Dialogs.ShowErrorAsync(msg);
            }

        }

        [RelayCommand]
        private async Task ClearLocalDatabase()
        {
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
                var msg = $"Error in SettingsViewModel:ClearLocalDatabase:{ex.Message}";
                await Dialogs.ShowErrorAsync(msg);
            }

        }

        [RelayCommand]
        private async Task ClearRemoteDatabase()
        {
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
                var msg = $"Error in SettingsViewModel:ClearRemoteDatabase:{ex.Message}";
                await Dialogs.ShowErrorAsync(msg);
            }

        }

        [RelayCommand]
        private async Task FactoryReset()
        {
            try
            {
                if (!await Dialogs.ConfirmAsync(
                        "Factory Reset?",
                        "This will clear the local and remote database and clear settings.  This cannot be undone"))
                    return;

                await _remoteDatabase.DeleteUserDataAsync();

                await _localDatabase.DeleteDatabaseAsync();

                _applicationSettingsService.ResetToFactory();

                SecureStorage.Default.Remove("supabase-session");

                await Shell.Current.GoToAsync("//Welcome");
            }
            catch (Exception ex)
            {
                var msg = $"Error in SettingsViewModel:FactoryReset:{ex.Message}";
                await Dialogs.ShowErrorAsync(msg);
            }

        }

        [RelayCommand]
        private async Task ChangeSignIn()
        {
            try
            {
                await Shell.Current.GoToAsync("//Welcome");
            }
            catch (Exception ex)
            {
                var msg = $"Error in SettingsViewModel:ChangeSignIn:{ex.Message}";
                await Dialogs.ShowErrorAsync(msg);
            }

        }

        public async Task InitialiseAsync()
        {
            try
            {
                IsLoggedIn = await _authenticationService.IsLoggedInAsync();
            }
            catch (Exception ex)
            {
                var msg = $"Error in SettingsViewModel:InitialiseAsync:{ex.Message}";
                await Dialogs.ShowErrorAsync(msg);
            }

        }

    }
}
