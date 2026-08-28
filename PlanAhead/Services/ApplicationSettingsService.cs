using Microsoft.Maui.Storage;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Infrastructure.Services;

public class ApplicationSettingsService
    : IApplicationSettingsService
{
    private const string FirstRunKey = "FirstRun";
    private const string SyncModeKey = "SyncMode";

    public bool IsFirstRun
    {
        get => Preferences.Default.Get("FirstRun", true);
        set => Preferences.Default.Set("FirstRun", value);
    }

    public long LastRemoteSyncVersion
    {
        get
        {
            return Preferences.Default.Get("LastRemoteSyncVersion", 0);
        }
        set
        {
            Preferences.Default.Set("LastRemoteSyncVersion", value);
        }
    }

    public long LastLocalSyncVersion
    {
        get
        {
            return Preferences.Default.Get("LastLocalSyncVersion", 0);
        }
        set
        {
            Preferences.Default.Set("LastLocalSyncVersion", value);
        }
    }

    public long LastLocalVersion
    {
        get
        {
            return Preferences.Default.Get("LastLocalVersion", 0);
        }
        set
        {
            Preferences.Default.Set("LastLocalVersion", value);
        }
    }

    public DateTime LastRemoteSyncUtc
    {
        get { 
            return Preferences.Default.Get("LastRemoteSyncUtc", new DateTime(2000, 1, 1)); 
        }
        set {
            Preferences.Default.Set("LastRemoteSyncUtc", value); 
        }
    }

    public DateTime LastLocalSyncUtc
    {
        get
        {
            return Preferences.Default.Get("LastLocalSyncUtc", new DateTime(2000, 1, 1));
        }
        set
        {
            Preferences.Default.Set("LastLocalSyncUtc", value);
        }
    }

    public DateTime LastLocalUtc
    {
        get
        {
            return Preferences.Default.Get("LastLocalUtc", new DateTime(2000, 1, 1));
        }
        set
        {
            Preferences.Default.Set("LastLocalUtc", value);
        }
    }

    public SyncMode SyncMode
    {
        get
        {
            var value =
                Preferences.Default.Get(
                    SyncModeKey,
                    (int)SyncMode.Offline);

            return (SyncMode)value;
        }

        set =>
            Preferences.Default.Set(
                SyncModeKey,
                (int)value);
    }

    public void MarkFirstRunComplete()
    {
        Preferences.Default.Set(FirstRunKey, false);
    }

    public void ResetToFactory()
    {
        IsFirstRun = true;
        LastRemoteSyncVersion = 0;
        LastLocalSyncVersion = 0;
        LastLocalVersion = 0;
        LastRemoteSyncUtc = new DateTime(2001, 1, 1 );
        LastLocalSyncUtc = new DateTime(2001, 1, 1);
        LastLocalUtc = new DateTime(2001, 1, 1);

    }
}