using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Services.Funds;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels;

public partial class DiagnosticsViewModel : BaseViewModel
{
    private readonly ILogService _logService;

    [ObservableProperty]
    private string log = string.Empty;

    public DiagnosticsViewModel(
        INavigationService navigation,
        IDialogService dialogs, 
        ILogService logService)
        : base(navigation, dialogs)
    {
        _logService = logService;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            Log = await _logService.GetLogAsync();
        }
        catch (Exception ex)
        {
            var msg = $"Error in DiagnosticsViewModel:InitialiseAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        Log = await _logService.GetLogAsync();
    }

    [RelayCommand]
    public async Task ClearAsync()
    {
        await _logService.ClearAsync();
        Log = string.Empty;
    }
}