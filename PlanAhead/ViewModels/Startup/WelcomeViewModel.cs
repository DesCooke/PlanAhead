using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels.Accounts;
using PlanAhead.Views;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;
using System.Collections.ObjectModel;

namespace PlanAhead.ViewModels.Funds;

public partial class WelcomeViewModel : BaseViewModel
{
    private readonly IApplicationSettingsService _settings;
    private readonly IAuthenticationService _authentication;

    public WelcomeViewModel(
        IApplicationSettingsService settings,
        IAuthenticationService authentication,
        INavigationService navigation,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _settings = settings;
        _authentication = authentication;
    }

    [RelayCommand]
    private async Task OfflineAsync()
    {
        _settings.SyncMode = SyncMode.Offline;
        _settings.IsFirstRun = false;

        Application.Current!.MainPage = new AppShell();
    }

    [RelayCommand]
    private async Task OnlineManualAsync()
    {
        _settings.SyncMode = SyncMode.SupabaseManual;
        _settings.IsFirstRun = false;

        Application.Current!.MainPage = new AppShell();
    }

    [RelayCommand]
    private async Task OnlineAutoAsync()
    {
        _settings.SyncMode = SyncMode.SupabaseAuto;
        _settings.IsFirstRun = false;

        Application.Current!.MainPage = new AppShell();
    }
}