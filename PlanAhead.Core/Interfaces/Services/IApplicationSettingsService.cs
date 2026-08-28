using PlanAhead.Core.Models.Enums;
namespace PlanAhead.Core.Interfaces.Services;

public interface IApplicationSettingsService
{
    bool IsFirstRun { get; set; }

    //
    // Remote DB controls
    // - LastRemoteSyncUtc - last date/time the DBs were sync'd
    // - LastRemoteSyncVersion - latest copy of the RemoteSyncVersion variable
    //   Each time the Supabase database is updated the SyncVersion in the Supabase DB
    //   is incremented.
    //   If this value is greater than the recorded value - we know something has changed
    //   so we sync - then update the local value
    //
    DateTime LastRemoteSyncUtc { get; set; }

    long LastRemoteSyncVersion { get; set; }

    DateTime LastLocalSyncUtc { get; set; }

    //
    // Local DB controls
    // - LastLocalSyncUtc - last date/time the DBs were sync'd
    // - LastLocalSyncVersion - latest copy of the LocalSyncVersion variable
    // - LastLocalVersion - current counter
    //   Each time the local database is updated the LocalVersion is increased
    //   If this value is greater than the recorded value - we know something has changed
    //   so we sync - then update the local value
    //
    DateTime LastLocalUtc { get; set; }

    long LastLocalSyncVersion { get; set; }

    long LastLocalVersion { get; set; }


    SyncMode SyncMode { get; set; }

    void MarkFirstRunComplete();

    void ResetToFactory();
}