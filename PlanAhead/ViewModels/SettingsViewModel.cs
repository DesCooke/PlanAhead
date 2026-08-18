using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Repositories;
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

        [ObservableProperty]
        private bool isLoggedIn = false;

        public SettingsViewModel(INavigationService navigation,
            IDialogService dialogs, IAuthenticationService authenticationService) : base(navigation, dialogs)
        {
            _authenticationService = authenticationService;
        }


        [RelayCommand]
        private async Task DeveloperTools()
        {
            await Shell.Current.GoToAsync("DeveloperToolsPage");
        }


        [RelayCommand]
        private async Task LogoutAsync()
        {
            if (!await Dialogs.ConfirmAsync(
                    "Logout",
                    "Are you sure you want to logout?"))
                return;

            await _authenticationService.LogoutAsync();

            SecureStorage.Default.Remove("supabase-session");

            await Shell.Current.GoToAsync("//Login");
        }

        [RelayCommand]
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
