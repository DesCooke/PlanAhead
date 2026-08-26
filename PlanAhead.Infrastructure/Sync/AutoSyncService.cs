using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.DB;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Sync.Models;
using System.Diagnostics;

namespace PlanAhead.Infrastructure.Sync;

public class AutoSyncService : IAutoSyncService, IDisposable
{
    private readonly IApplicationSettingsService _settings;
    private readonly IAuthenticationService _authenticationService;

    private readonly IEnumerable<IEntitySynchroniser> _synchronisers;
    private readonly INetworkService _networkService;
    private readonly SQLiteContext _context;
    private readonly ISyncService _syncService;
    private readonly ISyncStateService _syncStateService;
    private readonly ISyncStatusService _syncStatusService;

    private CancellationTokenSource? _cts;
    private Task? _worker;
    private Guid _userId;
    
    public bool IsRunning => _worker != null;
    

    public AutoSyncService(
        IApplicationSettingsService settings,
        IEnumerable<IEntitySynchroniser> synchronisers,
        INetworkService networkService,
        IAuthenticationService authenticationService,
        SQLiteContext context,
        ISyncService syncService,
        ISyncStateService syncStateService,
        ISyncStatusService syncStatusService)
    {
        _settings = settings;
        _synchronisers = synchronisers;
        _networkService = networkService;
        _authenticationService = authenticationService;
        _context = context;
        _syncService = syncService;
        _syncStateService = syncStateService;
        _syncStatusService = syncStatusService;
    }

    public void Start(Guid userId)
    {
        if (_worker != null)
            return;

        // need to pass in userId to remove circular dependencies
        _userId = userId;

        _cts = new CancellationTokenSource();

        _worker = Task.Run(() => WorkerAsync(_cts.Token));
    }

    private async Task WorkerAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                Debug.WriteLine("Syncing Start");
                try
                {
//                    if (await _syncStateService.HasRemoteChangesAsync(_userId))
  //                  {
                        await _syncService.SyncAsync(_userId, token);

                        await _syncStateService.MarkAsUptodateAsync(_userId);
    //                }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                // just in case the check is superfast - make it atleast 2 seconds so we see 
                // something in the UI
                Debug.WriteLine("Syncing End");
//                await Task.Delay(TimeSpan.FromSeconds(2), token);
            }
            finally
            {
            }
            await Task.Delay(TimeSpan.FromSeconds(10), token);
        }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
    public async Task StopAsync()
    {
        if (_cts == null)
            return;

        _cts.Cancel();

        try
        {
            await _worker!;
        }
        catch (OperationCanceledException)
        {
        }

        _worker = null;

        _cts.Dispose();
        _cts = null;
    }

    private async Task<long> GetRemoteSyncVersionAsync()
    {

        var db = await _context.GetConnectionAsync();

        var response = await db
            .Table<UserSyncRecord>()
            .Where(x => x.UserId == _userId)
            .FirstOrDefaultAsync();

        if (response != null)
            return response.SyncVersion;
        return 0;
    }

    public async Task<bool> AutoSyncAsync(
        CancellationToken cancellationToken = default)
    {
        if (!_networkService.IsConnected)
            return false;

        var remoteVersion = await GetRemoteSyncVersionAsync();

        if (remoteVersion == _settings.LastSyncVersion)
            return true;

        await _syncService.SyncAsync(_userId);

        remoteVersion = await GetRemoteSyncVersionAsync();
        _settings.LastSyncVersion = remoteVersion;

        return true;
    }

}