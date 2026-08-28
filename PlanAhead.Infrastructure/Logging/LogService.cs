using PlanAhead.Core.Interfaces.Services;
using System.Diagnostics;
using System.Text;

namespace PlanAhead.Infrastructure.Logging;

public class LogService : ILogService
{
    private readonly string _logFile;

    private readonly SemaphoreSlim _lock = new(1, 1);

    public LogService()
    {
        _logFile = Path.Combine(
            FileSystem.AppDataDirectory,
            "planahead.log");
    }

    private string getTime()
    {
        return $"{DateTime.Now:HH:mm:ss.fff} UTC";
    }

    public async Task LogAsync(string message)
    {
        var line = $"{getTime()}: {message}";

        await _lock.WaitAsync();

        try
        {
            Debug.WriteLine(line);
            await File.AppendAllTextAsync(
                _logFile,
                line + Environment.NewLine);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task LogExceptionAsync(
        Exception ex,
        string? context = null)
    {
        await _lock.WaitAsync();

        try
        {
            var sb = new StringBuilder();

            sb.Append($"{getTime()}:");

            if (!string.IsNullOrWhiteSpace(context))
                sb.Append(context).Append(": ");

            sb.Append(ex);

            Debug.WriteLine(sb.ToString());

            await LogAsync(sb.ToString());
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<string> GetLogAsync()
    {
        await _lock.WaitAsync();

        try
        {
            if (!File.Exists(_logFile))
                return string.Empty;

            return await File.ReadAllTextAsync(_logFile);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ClearAsync()
    {
        await _lock.WaitAsync();

        try
        {
            if (File.Exists(_logFile))
                File.Delete(_logFile);
        }
        finally
        {
            _lock.Release();
        }
    }
}