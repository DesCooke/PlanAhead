using CommunityToolkit.Mvvm.ComponentModel;
using PlanAhead.Interfaces;

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
}