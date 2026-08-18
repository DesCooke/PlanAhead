using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.Sync
{
    public interface ISyncService
    {
        bool IsSyncing { get; }

        Task<bool> SyncAsync(Guid userId,
            CancellationToken cancellationToken = default);
    }
}
