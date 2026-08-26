using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;

namespace PlanAhead.Infrastructure.Sync;

public partial class SyncStatusService : ObservableObject, ISyncStatusService
{
    private bool _isSyncing;

    public bool IsSyncing
    {
        get => _isSyncing;
        set
        {
            _isSyncing = value;
        }
    }
}