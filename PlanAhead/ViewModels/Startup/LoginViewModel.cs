using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Interfaces;
using PlanAhead.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private IApplicationSettingsService _settings;

    public LoginViewModel(
        IApplicationSettingsService settings,
        IAuthenticationService authentication,
        INavigationService navigation,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _settings = settings;
    }


    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [RelayCommand]
    private async Task LoginAsync()
    {
        //await _authenticationService.SignInAsync(Email, Password);

        Application.Current!.MainPage = new AppShell();
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
//        await _authenticationService.SignUpAsync(Email, Password);

        Application.Current!.MainPage = new AppShell();
    }

    
    [RelayCommand]
    private async Task OfflineOnlyAsync()
    {
        _settings.SyncMode = PlanAhead.Core.Models.Enums.SyncMode.Offline;
        Application.Current!.MainPage = new AppShell();

    }

    [RelayCommand]
    private async Task GoogleAsync()
    {

    }

    [RelayCommand]
    private async Task CancelAsync()
    {

    }
}