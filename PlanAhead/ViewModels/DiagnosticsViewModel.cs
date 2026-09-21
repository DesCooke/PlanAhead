using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.MethodLogging;
using PlanAhead.Core.Services.Funds;
using PlanAhead.Interfaces;
using PlanAhead.Services;
using PlanAhead.ViewModels;
using System.Diagnostics;

[MethodLogging]
public partial class DiagnosticsViewModel : BaseViewModel
{

    private readonly ILogService _logService;

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

        OnPropertyChanged(nameof(Log));
    }

    [ObservableProperty]
    private string log = string.Empty;



    public DiagnosticsViewModel(
        INavigationService navigation,
        IDialogService dialogs, 
        ILogService logService)
        : base(navigation, dialogs, logService)
    {
        _logService = logService;
    }

    public async Task InitialiseAsync()
    {
        Log = await _logService.GetLogAsync();
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        Log = await _logService.GetLogAsync();
    }

    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    public async Task ClearAsync()
    {
        await LogService.ClearAsync();
        Log = await _logService.GetLogAsync();
    }
}