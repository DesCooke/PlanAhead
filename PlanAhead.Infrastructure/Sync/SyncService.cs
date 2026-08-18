using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Infrastructure.DB;
using PlanAhead.Infrastructure.Repositories;

namespace PlanAhead.Infrastructure.Sync;

public class SyncService : ISyncService
{
    private readonly IApplicationSettingsService _settings;

    private readonly IEnumerable<IEntitySynchroniser> _synchronisers;
    private readonly INetworkService _networkService;

    public bool IsSyncing { get; private set; }

    public SyncService(
        IApplicationSettingsService settings,
        IEnumerable<IEntitySynchroniser> synchronisers,
        INetworkService networkService)
    {
        _settings = settings;
        _synchronisers = synchronisers;
        _networkService = networkService;
    }

    public async Task<bool> SyncAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (IsSyncing)
            return false;

        if (!_networkService.IsConnected)
            return false;

        IsSyncing = true;

        try
        {
            foreach (var synchroniser in _synchronisers)
            {
                await synchroniser.UploadPendingAsync(userId);
            }

            foreach (var synchroniser in _synchronisers)
            {
                await synchroniser.DownloadChangesAsync(_settings.LastSyncUtc);
            }
            _settings.LastSyncUtc = DateTime.UtcNow;

            return true;
        }
        finally
        {
            IsSyncing = false;
        }
    }

}