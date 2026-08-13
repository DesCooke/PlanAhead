using CommunityToolkit.Maui.Extensions;
using PlanAhead.Interfaces;
using PlanAhead.Views.Popups;

namespace PlanAhead.Services;

public class DialogService
    : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task ShowMessageAsync(
        string title,
        string message)
    {
        return Shell.Current.DisplayAlertAsync(
            title,
            message,
            "OK");
    }

    public Task ShowErrorAsync(
        string message)
    {
        return Shell.Current.DisplayAlertAsync(
            "Error",
            message,
            "OK");
    }

    public Task<bool> ConfirmAsync(
        string title,
        string message)
    {
        return Shell.Current.DisplayAlertAsync(
            title,
            message,
            "Yes",
            "No");
    }

    public async Task<string?> PickIconAsync(string? currentIconId)
    {
        var popup = _serviceProvider.GetRequiredService<IconPickerPopup>();

        popup.Initialise(currentIconId);

        await Shell.Current.ShowPopupAsync(popup);

        return popup.SelectedIconId;
    }
}