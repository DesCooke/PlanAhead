using CommunityToolkit.Maui.Extensions;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Interfaces;
using PlanAhead.Views.Popups;

namespace PlanAhead.Services;

public class DialogService
    : IDialogService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;

    public DialogService(IServiceProvider serviceProvider, ILogService logService)
    {
        _serviceProvider = serviceProvider;
        _logService = logService;
    }

    private static Page? GetCurrentPage(Page? page)
    {
        while (true)
        {
            switch (page)
            {
                case Shell shell:
                    page = shell.CurrentPage;
                    break;

                case NavigationPage nav:
                    page = nav.CurrentPage;
                    break;

                case TabbedPage tab:
                    page = tab.CurrentPage;
                    break;

                default:
                    return page;
            }
        }
    }

    public Task ShowMessageAsync(
        string title,
        string message)
    {
        _logService.LogAsync($"Message shown: {title}, {message}");

        var page = GetCurrentPage(Application.Current?.Windows[0].Page);

        if (page != null)
        {
            return page.DisplayAlertAsync(
            title,
            message,
            "OK");
        }
        return Task.CompletedTask;
    }

    public Task ShowErrorAsync(
        string message)
    {
        _logService.LogAsync($"Error shown: {message}");

        var page = GetCurrentPage(Application.Current?.Windows[0].Page);

        if (page != null)
        {
            return page.DisplayAlertAsync(
            "Error",
            message,
            "OK");
        }
        return Task.CompletedTask;
    }

    public async Task<bool> ConfirmAsync(
        string title,
        string message)
    {
        await _logService.LogAsync($"Confirmation shown: {title} {message}");

        var page = GetCurrentPage(Application.Current?.Windows[0].Page);

        if (page != null)
        {
            return await page.DisplayAlertAsync(
            title,
            message,
            "Yes",
            "No");
        }
        return false;
        
    }

    public async Task<string?> PickIconAsync(string? currentIconId)
    {
        var popup = _serviceProvider.GetRequiredService<IconPickerPopup>();

        popup.Initialise(currentIconId);

        await Shell.Current.ShowPopupAsync(popup);

        return popup.SelectedIconId;
    }
}