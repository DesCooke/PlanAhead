using System.ComponentModel;

namespace PlanAhead.Infrastructure.Sync;

public interface ISyncStatusService : INotifyPropertyChanged
{
    bool IsSyncing { get; set; }
}