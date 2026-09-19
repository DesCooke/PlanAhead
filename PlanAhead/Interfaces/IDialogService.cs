using PlanAhead.Resources.Icons;

namespace PlanAhead.Interfaces;

public interface IDialogService
{
    Task ShowMessageAsync(
        string title,
        string message);

    Task ShowErrorAsync(
        string message);

    Task ShowException(
        Exception ex);

    Task<bool> ConfirmAsync(
        string title,
        string message);

    Task<string?> PickIconAsync(string? currentIcon);
}