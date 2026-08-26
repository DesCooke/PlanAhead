using Newtonsoft.Json.Linq;
using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Sync.Models;
using Supabase;
using System.Diagnostics;

namespace PlanAhead.Infrastructure.Sync;

public class SyncService : ISyncService
{
    private readonly IApplicationSettingsService _settings;
    private readonly IEnumerable<IEntitySynchroniser> _synchronisers;
    private readonly INetworkService _networkService;
    private readonly SQLiteContext _context;
    private readonly Client _supabase;
    private ISyncStatusService _syncStatusService;


    public SyncService(
        IApplicationSettingsService settings,
        IEnumerable<IEntitySynchroniser> synchronisers,
        INetworkService networkService,
        SQLiteContext context, 
        Client client,
        ISyncStatusService syncStatusService)
    {
        _settings = settings;
        _synchronisers = synchronisers;
        _networkService = networkService;
        _context = context;
        _supabase = client;
        _syncStatusService = syncStatusService;
    }

    private async Task<long> GetRemoteSyncVersionAsync(Guid userId)
    {

        var response = _supabase
            .From<UserSyncRecord>()
            .Where(x => x.UserId == userId)
            .Get();

        if (response != null && response.AsyncState!=null)
            return (long)(response.AsyncState);
        return 0;
    }

    public async Task<bool> SyncAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        /*
    Debug.WriteLine("SyncAsync Start");
    if (_syncStatusService.IsSyncing)
    {
        Debug.WriteLine("_syncStatusService.IsSyncing is true - so ignoring");
        return false;
    }

    if (!_networkService.IsConnected)
    {
        Debug.WriteLine("_networkService.IsConnected is false - so ignoring");
        return false;
    }

    _syncStatusService.IsSyncing = true;
    try
    {
        Debug.WriteLine($"Uploading changes since {_settings.LastSyncUtc}");
        foreach (var synchroniser in _synchronisers)
        {
            await synchroniser.UploadPendingAsync(userId);
        }

        Debug.WriteLine($"Downloading changes since {_settings.LastSyncUtc}");
        foreach (var synchroniser in _synchronisers)
        {
            await synchroniser.DownloadChangesAsync(_settings.LastSyncUtc);
        }

        _settings.LastSyncUtc = DateTime.UtcNow;
        Debug.WriteLine($"Setting LastSyncUtc to {_settings.LastSyncUtc}");
        await Task.Delay(5000);
        }
        finally
        {
            _syncStatusService.IsSyncing = false;
        }
        */
        return true;
    }

}