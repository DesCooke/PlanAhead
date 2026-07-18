namespace PlanAhead.Core.Interfaces;

public interface IDialogService
{
    Task ShowMessageAsync(
        string title,
        string message);

    Task ShowErrorAsync(
        string message);

    Task<bool> ConfirmAsync(
        string title,
        string message);
}