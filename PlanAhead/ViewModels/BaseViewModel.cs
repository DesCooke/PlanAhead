using CommunityToolkit.Mvvm.ComponentModel;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Logging;
using PlanAhead.Interfaces;

namespace PlanAhead.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    protected INavigationService Navigation { get; }
    protected IDialogService DialogService { get; }
    protected ILogService LogService { get; }

    protected BaseViewModel(
        INavigationService navigation,
        IDialogService dialogService, 
        ILogService logService)
    {
        Navigation = navigation;
        DialogService = dialogService;
        LogService = logService;
    }

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isRefreshing;

    protected void RefreshUi(params string[] properties)
    {
        foreach (var property in properties)
        {
            OnPropertyChanged(property);
        }
    }

    protected async Task ExecuteBusyAsync(Func<Task> action)
    {
        try
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await action();
            }
            finally
            {
                IsBusy = false;
            }
        }
        catch (Exception ex)
        {
            LogService.LogException(ex);
            await DialogService.ShowException(ex);
        }
    }
}