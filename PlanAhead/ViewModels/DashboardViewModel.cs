using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PlanAhead.Core.Constants;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.Views;
using PlanAhead.Views.Startup;
using Supabase;
using PlanAhead.Core.Messaging;
using PlanAhead.Infrastructure.Sync;

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

    private readonly AccountRepository _repository;
    private readonly IApplicationSettingsService _settings;
    private readonly Client _client;
    private readonly IAuthenticationService _authenticationService;
    private readonly IApplicationStartupService _startupService;
    private readonly ISyncService _syncService;

    public DashboardViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs, 
        Client client,
        IAuthenticationService authenticationService,
        IApplicationStartupService startupService,
        IApplicationSettingsService settings, 
        ISyncService syncService): base (navigation, dialogs)
    {
        _repository = repository;
        _settings = settings;
        _client = client;
        _authenticationService  = authenticationService;
     
        _startupService = startupService;
        _syncService = syncService;
    }

    [RelayCommand]
    private async Task SyncAsync()
    {
        try
        {
            var userIdString = await _authenticationService.GetCurrentUserIdAsync();
            if (userIdString != null)
            {
                var userId = Guid.Parse(userIdString);
                if(userId != Guid.Empty)
                    await _syncService.SyncAsync(userId);
            }
        }
        catch (Exception ex)
        {
            var msg = $"Error in DashboardViewModel:SyncAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private async Task TestRepository()
    {
        try
        {
            var accounts = await _repository.GetAllAsync();

            System.Diagnostics.Debug.WriteLine(
                $"Number of accounts = {accounts.Count}");
        }
        catch (Exception ex)
        {
            var msg = $"Error in DashboardViewModel:TestRepository:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }


    public async Task RefreshAsync()
    {
        try
        {
            WeakReferenceMessenger.Default.Send(new SyncStatusChangedMessage());
        }
        catch (Exception ex)
        {
            var msg = $"Error in DashboardViewModel:RefreshAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }
}