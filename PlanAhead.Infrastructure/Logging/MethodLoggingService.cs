using PlanAhead.Core.Interfaces.Services;

namespace PlanAhead.Infrastructure.Logging;

public static class MethodLoggingService
{
    private static ILogService? _logService;

    private static readonly AsyncLocal<int> _depth = new();

    public static void Configure(
        ILogService logService)
    {
        _logService = logService;
    }

    public static int Depth
    {
        get => _depth.Value;
        set => _depth.Value = value;
    }

    public static string Indent()
    {
        return new string(
            ' ',
            _depth.Value * 2);
    }

    public static void Write(
        string message)
    {
        _logService?.Log(
            $"{Indent()}{message}");
    }
}