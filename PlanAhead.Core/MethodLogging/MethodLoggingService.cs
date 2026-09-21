using PlanAhead.Core.Interfaces.Services;
using System.Diagnostics;

namespace PlanAhead.Core.MethodLogging;

public static class MethodLoggingService
{
    private static ILogService? _logService;

    private static readonly AsyncLocal<int> _depth = new();

    private static Func<Exception, Task>? _exceptionHandler;

    public static int Depth
    {
        get => _depth.Value;
        set => _depth.Value = value;
    }

    public static void Configure(ILogService logService)
    {
        _logService = logService;
    }

    public static void SetExceptionHandler(
        Func<Exception, Task> exceptionHandler)
    {
        _exceptionHandler = exceptionHandler;
    }

    public static string Indent()
    {
        return new string(' ', _depth.Value * 2);
    }

    public static void Write(string message)
    {
        _logService?.Log($"{Indent()}{message}");
    }

    /// <summary>
    /// Called by the synchronous Fody OnException callback.
    /// This method must remain synchronous.
    /// </summary>
    public static void HandleException(Exception exception)
    {
        var handler = _exceptionHandler;

        if (handler == null)
        {
            Debug.WriteLine(
                $"MethodLoggingService: No exception handler registered: {exception.Message}");

            return;
        }

        // Do not await here.
        //
        // Fody's OnException is synchronous and this method may be called
        // while an exception is unwinding through the UI thread.
        //
        // Queue the handler so that it runs independently of the Fody
        // exception callback.
        _ = RunExceptionHandlerAsync(handler, exception);
    }

    private static async Task RunExceptionHandlerAsync(
        Func<Exception, Task> handler,
        Exception exception)
    {
        try
        {
            // Give MAUI time to complete the current exception/navigation
            // processing before attempting to display UI.
            await Task.Delay(1000);

            await handler(exception);
        }
        catch (Exception ex)
        {
            // An exception while displaying an exception must never
            // bring down the application.
            Debug.WriteLine(
                $"MethodLoggingService: Exception handler failed: {ex}");
        }
    }
}