using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PlanAhead.Core.Interfaces.Services;

public interface ILogService
{
    void Log(string message);

    IReadOnlyList<string> Lines { get; }

    Task LogAsync(string message);

    void LogException(
        Exception exception,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "");

    Task<string> GetLogAsync();

    Task ClearAsync();
}