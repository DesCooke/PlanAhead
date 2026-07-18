using PlanAhead.core.Interfaces;

namespace PlanAhead.Services;

public class DialogService
    : IDialogService
{
    public Task ShowMessageAsync(
        string title,
        string message)
    {
        return Shell.Current.DisplayAlert(
            title,
            message,
            "OK");
    }

    public Task ShowErrorAsync(
        string message)
    {
        return Shell.Current.DisplayAlert(
            "Error",
            message,
            "OK");
    }

    public Task<bool> ConfirmAsync(
        string title,
        string message)
    {
        return Shell.Current.DisplayAlert(
            title,
            message,
            "Yes",
            "No");
    }
}