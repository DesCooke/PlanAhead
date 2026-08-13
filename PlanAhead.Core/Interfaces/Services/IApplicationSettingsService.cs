using PlanAhead.Core.Models.Enums;
namespace PlanAhead.Core.Interfaces.Services;

public interface IApplicationSettingsService
{
    bool IsFirstRun { get; set; }

    SyncMode SyncMode { get; set; }

    void MarkFirstRunComplete();
}