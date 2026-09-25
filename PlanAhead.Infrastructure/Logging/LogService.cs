using PlanAhead.Core.Interfaces.Services;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;

namespace PlanAhead.Infrastructure.Logging;

public class LogService : ILogService
{
    private readonly List<string> _lines = [];

    public IReadOnlyList<string> Lines => _lines;

    private readonly object _lock = new();

    public LogService()
    {
    }

    private string GetTime()
    {
        var threadId = Environment.CurrentManagedThreadId;

        return $"{DateTime.Now:HH:mm:ss.fff} [T{threadId:D4}]";
    }
    public void Log(string message)
    {
        var line = $"{GetTime()}: {message}";

        lock (_lock)
        {
            _lines.Add(line);
        }
    }

    public Task LogAsync(string message)
    {
        Log(message);

        return Task.CompletedTask;
    }

    public void LogException(
        Exception exception,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "")
    {
        var unitName = System.IO.Path.GetFileNameWithoutExtension(filePath).Split('/', '\\').Last();

        var message =
            $"Error in {unitName}:{memberName}:{exception.Message}";

        Log(message);
    }

    public Task<string> GetLogAsync()
    {
        lock (_lock)
        {
            return Task.FromResult(
                string.Join("\r\n", _lines));
        }
    }

    public Task ClearAsync()
    {
        lock (_lock)
        {
            _lines.Clear();
        }

        return Task.CompletedTask;
    }
}