using PlanAhead.Core.Models.Enums;
namespace PlanAhead.Core.Interfaces.Services;

public interface IApplicationSettingsService
{
    bool IsFirstRun { get; set; }

    DateTime LastSyncUtc { get; set; }

    long LastSyncVersion { get; set; }

    SyncMode SyncMode { get; set; }

    void MarkFirstRunComplete();

    void ResetToFactory();
}