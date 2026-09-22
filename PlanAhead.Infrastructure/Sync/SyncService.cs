using Newtonsoft.Json.Linq;
using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.MethodLogging;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.Logging;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Sync.Models;
using Supabase;
using System.Diagnostics;

namespace PlanAhead.Infrastructure.Sync;

[MethodLogging]
public class SyncService : ISyncService
{
    private readonly IApplicationSettingsService _settings;
    private readonly IEnumerable<IEntitySynchroniser> _synchronisers;
    private readonly INetworkService _networkService;
    private readonly SQLiteContext _context;
    private readonly Client _supabase;
    private ISyncStatusService _syncStatusService;
    private readonly ILogService _logService;

    public SyncService(
        IApplicationSettingsService settings,
        IEnumerable<IEntitySynchroniser> synchronisers,
        INetworkService networkService,
        SQLiteContext context, 
        Client client,
        ISyncStatusService syncStatusService,
        ILogService logService,
        ISyncStateService syncStateService)
    {
        _settings = settings;
        _synchronisers = synchronisers;
        _networkService = networkService;
        _context = context;
        _supabase = client;
        _syncStatusService = syncStatusService;
        _logService = logService;
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
        bool hasRemoteChanges,
        bool hasLocalChanges,
        CancellationToken cancellationToken = default)
    {
        if (_syncStatusService.IsSyncing)
        {
            MethodLoggingService.Write($"  _syncStatusService.IsSyncing is true - so ignoring");
        }
        else
        {
            MethodLoggingService.Write($"  Setting _syncStatusService.IsSyncing to true");
            _syncStatusService.IsSyncing = true;
            try
            {
                if (!_networkService.IsConnected)
                {
                    MethodLoggingService.Write($"  _networkService.IsConnected is false - so ignoring");
                    MethodLoggingService.Write($"  Setting _syncStatusService.IsSyncing to false");
                    _syncStatusService.IsSyncing = false;
                    MethodLoggingService.Write($"  .._syncStatusService.IsSyncing is {_syncStatusService.IsSyncing}");
                }
                else
                {

                    if (hasLocalChanges)
                    {
                        MethodLoggingService.Write($"  Uploading local changes");
                        foreach (var synchroniser in _synchronisers)
                        {
                            await synchroniser.UploadPendingAsync(userId);
                        }
                        _settings.LastLocalSyncUtc = DateTime.UtcNow;
                        _settings.LastLocalSyncVersion = _settings.LastLocalVersion;
                    }
                    else
                    {
                        MethodLoggingService.Write($"  hasLocalChanges is false for this device");
                    }


                    if (hasRemoteChanges)
                    {
                        MethodLoggingService.Write($"  Downloading changes since {_settings.LastRemoteSyncUtc}");
                        foreach (var synchroniser in _synchronisers)
                        {
                            await synchroniser.DownloadChangesAsync(_settings.LastRemoteSyncUtc);
                        }
                    }
                    else
                    {
                        MethodLoggingService.Write($"  hasRemoteChanges is false for this user");
                    }
                }
            }
            finally
            {
                MethodLoggingService.Write($"  Setting _syncStatusService.IsSyncing to false");
                _syncStatusService.IsSyncing = false;
                MethodLoggingService.Write($"  .._syncStatusService.IsSyncing is {_syncStatusService.IsSyncing}");
            }

        }
        return true;
    }

}