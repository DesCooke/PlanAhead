using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.Sync
{
    public interface ISyncService
    {
        Task<bool> SyncAsync(Guid userId,
            bool hasRemoteChanges,
            bool hasLocalChanges,
            CancellationToken cancellationToken = default);
    }
}
