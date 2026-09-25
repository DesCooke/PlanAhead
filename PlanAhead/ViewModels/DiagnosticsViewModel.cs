using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Logging;
using PlanAhead.Core.Services.Funds;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels;

public partial class DiagnosticsViewModel : BaseViewModel
{

    private readonly ILogService _logService;

    private bool _initialised;

    private string _filter = string.Empty;

    public string Filter
    {
        get => _filter;
        set
        {
            if (_filter == value)
                return;

            _filter = value;
            OnPropertyChanged();

            UpdateLog();
        }
    }

    private void UpdateLog()
    {
        var lines = _logService.Lines;

        if (string.IsNullOrWhiteSpace(Filter))
        {
            Log = string.Join(Environment.NewLine, lines);
        }
        else
        {
            Log = string.Join(
                Environment.NewLine,
                lines.Where(x =>
                    x.Contains(Filter, StringComparison.OrdinalIgnoreCase)));
        }
    }
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
        if (_initialised)
            return;

        _initialised = true;

        using var log = MethodLoggingService.Begin();

        try
        {
            Log = await _logService.GetLogAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            await Dialogs.ShowExceptionAsync(ex);
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