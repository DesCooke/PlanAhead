using System.Diagnostics;

namespace PlanAhead.Core.Logging;

public sealed class MethodLogScope : IDisposable
{
    private readonly string _filePath;
    private readonly string _methodName;
    private readonly int _depth;
    private readonly Stopwatch _stopwatch;

    private bool _disposed;

    internal MethodLogScope(
        string filePath,
        string methodName,
        int depth)
    {
        _filePath = filePath;
        _methodName = methodName;
        _depth = depth;

        _stopwatch = Stopwatch.StartNew();

        MethodLoggingService.Write(
            $"{MethodLoggingService.Indent(_depth)}START {_filePath}.{_methodName}");

        MethodLoggingService.IncreaseDepth();
    }

    public void Exception(Exception exception)
    {
        MethodLoggingService.Write(
            $"{MethodLoggingService.Indent(_depth + 1)}EXCEPTION {_filePath}.{_methodName}: {exception.Message}");

        MethodLoggingService.Write(
            $"{MethodLoggingService.Indent(_depth + 1)}{exception}");
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        _stopwatch.Stop();

        MethodLoggingService.DecreaseDepth();

        MethodLoggingService.Write(
            $"{MethodLoggingService.Indent(_depth)}END  {_filePath}.{_methodName} ({_stopwatch.Elapsed.TotalMilliseconds:N0} ms)");
    }
}