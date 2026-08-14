using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Constants;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.Views;
using PlanAhead.Views.Startup;
using Supabase;

namespace PlanAhead.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    [ObservableProperty]
    private string title = AppConstants.ApplicationName;

    [ObservableProperty]
    private string welcomeMessage =
        "Welcome to Plan Ahead";

    [ObservableProperty]
    private string version =
        $"Version {AppConstants.Version}";

    [ObservableProperty]
    private string syncMethod =
        "<Sync Method>";

    [ObservableProperty]
    private string currentSupabaseUser =
        "<user>>";

    private readonly AccountRepository _repository;
    private readonly IApplicationSettingsService _settings;
    private readonly Client _client;
    private readonly IAuthenticationService _authenticationService;
    private readonly IApplicationStartupService _startupService;

    public DashboardViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs, 
        Client client,
        IAuthenticationService authenticationService,
        IApplicationStartupService startupService,
        IApplicationSettingsService settings): base (navigation, dialogs)
    {
        _repository = repository;
        _settings = settings;
        _client = client;
        _authenticationService  = authenticationService;
        _startupService = startupService;
        syncMethod = settings.SyncMode.ToString();
        if(_client!=null && _client.Auth!= null && _client.Auth.CurrentUser!=null && _client.Auth.CurrentUser.Email != null)
            currentSupabaseUser = _client.Auth.CurrentUser.Email;

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
    private async Task TestRepository()
    {
        var accounts = await _repository.GetAllAsync();

        System.Diagnostics.Debug.WriteLine(
            $"Number of accounts = {accounts.Count}");
    }
}