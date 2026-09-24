using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Services;

public interface ILogService
{
    void Log(string message);

    Task LogAsync(string message);

    Task LogExceptionAsync(
        Exception ex,
        string? context = null);

    Task<string> GetLogAsync();

    Task ClearAsync();
}