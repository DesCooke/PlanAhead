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
}