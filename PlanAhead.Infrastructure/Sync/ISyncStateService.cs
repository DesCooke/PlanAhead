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

        Task UpdateRemoteSyncVersionAsync(Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> HasLocalChangesAsync(CancellationToken cancellationToken = default);

        Task<long> GetLocalSyncVersionAsync(CancellationToken cancellationToken = default);

        Task UpdateLocalSyncVersionAsync(CancellationToken cancellationToken = default);

        Task IncreaseLocalVersion(CancellationToken cancellationToken = default);
    }
}