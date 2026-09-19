using PlanAhead.Core.Interfaces.Services;
using System.Text;

namespace PlanAhead.Infrastructure.Logging;

public class LogService : ILogService
{
    private readonly List<string> _lines = [];

    private readonly object _lock = new();

    public LogService()
    {
    }

    private string GetTime()
    {
        return $"{DateTime.Now:HH:mm:ss.fff}";
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

    public Task LogExceptionAsync(
        Exception ex,
        string? context = null)
    {
        var sb = new StringBuilder();

        sb.Append($"{GetTime()}: ");
        sb.Append("(***EXCEPTION***): ");

        if (!string.IsNullOrWhiteSpace(context))
        {
            sb.Append(context).Append(": ");
        }

        sb.Append(ex);

        lock (_lock)
        {
            _lines.Add(sb.ToString());
        }

        return Task.CompletedTask;
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