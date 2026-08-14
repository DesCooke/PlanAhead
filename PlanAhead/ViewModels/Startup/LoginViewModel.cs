using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
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

    public LoginViewModel(
        IApplicationSettingsService settings,
        IAuthenticationService authenticationService,
        INavigationService navigation,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _settings = settings;
        _authenticationService = authenticationService;
    }


    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            var response = await _authenticationService.LoginAsync(Email, Password);

            await SecureStorage.Default.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(response));

            await Shell.Current.GoToAsync("//Dashboard");
        }
        catch (Exception ex)
        {
            await Dialogs.ShowErrorAsync(
                $"Unable to login to your account.  {ex.Message}");
        }
    }


    public async Task InitialiseAsync()
    {
        Email = "";
        Password = "";
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        try
        {
            await _authenticationService.RegisterAsync(
                Email,
                Password);



            var response = await _authenticationService.LoginAsync(Email, Password);

            await SecureStorage.Default.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(response));

            await Dialogs.ShowMessageAsync(
                "Registration",
                "Your account has been created and logged in.");

            await Shell.Current.GoToAsync("//Dashboard");
        }
        catch (Exception ex)
        {
                await Dialogs.ShowErrorAsync(
                    $"Unable to create your account.  {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task OfflineOnlyAsync()
    {
        _settings.SyncMode = PlanAhead.Core.Models.Enums.SyncMode.Offline;

        await Shell.Current.GoToAsync("//Dashboard");
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