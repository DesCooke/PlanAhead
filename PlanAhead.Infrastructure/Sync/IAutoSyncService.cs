using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.Sync
{
    public interface IAutoSyncService
    {
        void Start(Guid userId);

        Task StopAsync();

        Task<bool> AutoSyncAsync(
            CancellationToken cancellationToken = default);
    }
}
