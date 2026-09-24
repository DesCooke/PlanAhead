using System.Diagnostics;
using System.Runtime.CompilerServices;
using PlanAhead.Core.Interfaces.Services;

namespace PlanAhead.Core.Logging;

public static class MethodLoggingService
{
    private static ILogService? _logService;

    private static readonly AsyncLocal<int> _depth = new();

    public static void Configure(ILogService logService)
    {
        _logService = logService;
    }

    public static MethodLogScope Begin(
        [CallerMemberName] string methodName = "",
        [CallerFilePath] string filePath = "")
    {
        const string projectName = "PlanAhead"; 
        
        var projectIndex = filePath.IndexOf(
            projectName,
            StringComparison.OrdinalIgnoreCase);

        var relativePath = projectIndex >= 0
            ? filePath[projectIndex..]
            : filePath;

        return new MethodLogScope(
            relativePath,
            methodName,
            _depth.Value);
    }

    public static void Write(string message)
    {
        _logService?.Log(message);
    }

    internal static string Indent(int depth)
    {
        return new string(' ', depth * 2);
    }

    internal static void IncreaseDepth()
    {
        _depth.Value++;
    }

    internal static void DecreaseDepth()
    {
        if (_depth.Value > 0)
            _depth.Value--;
    }
}