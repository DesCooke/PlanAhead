using CommunityToolkit.Mvvm.ComponentModel;
using PlanAhead.Interfaces;

namespace PlanAhead.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    protected INavigationService Navigation { get; }
    protected IDialogService Dialogs { get; }

    protected BaseViewModel(
        INavigationService navigation,
        IDialogService dialogs)
    {
        Navigation = navigation;
        Dialogs = dialogs;
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
}