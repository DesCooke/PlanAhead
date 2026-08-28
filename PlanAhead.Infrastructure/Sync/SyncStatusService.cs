using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;

namespace PlanAhead.Infrastructure.Sync;

public partial class SyncStatusService : ObservableObject, ISyncStatusService
{
    [ObservableProperty]
    private bool isSyncing;

}