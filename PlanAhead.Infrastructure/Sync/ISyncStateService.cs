using System.Threading;
using System.Threading.Tasks;

namespace PlanAhead.Infrastructure.Sync
{
    public interface ISyncStateService
    {
        Task<bool> HasRemoteChangesAsync(Guid userId,
            CancellationToken cancellationToken = default);

        Task<long> GetRemoteSyncVersionAsync(Guid userId,
            CancellationToken cancellationToken = default);

        Task MarkAsUptodateAsync(Guid userId,
            CancellationToken cancellationToken = default);

    }
}