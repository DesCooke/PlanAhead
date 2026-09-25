using PlanAhead.Resources.Icons;
using System.Runtime.CompilerServices;

namespace PlanAhead.Interfaces;

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

    Task ShowExceptionAsync(
        Exception ex,
        [CallerFilePath] string filePath = "");

    Task<string?> PickIconAsync(string? currentIcon);
}