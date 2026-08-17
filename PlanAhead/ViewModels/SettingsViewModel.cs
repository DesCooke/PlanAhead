using CommunityToolkit.Mvvm.Input;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {

        public SettingsViewModel(INavigationService navigation,
            IDialogService dialogs) : base(navigation, dialogs)
        {
        }

        [RelayCommand]
        private async Task DeveloperTools()
        {
            await Shell.Current.GoToAsync("DeveloperToolsPage");
        }

        [RelayCommand]
        private async Task ChangeSignIn()
        {
            await Shell.Current.GoToAsync("//Welcome");
        }

    }
}
