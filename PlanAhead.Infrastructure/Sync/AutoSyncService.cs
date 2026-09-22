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
using System.Diagnostics;

namespace PlanAhead.Infrastructure.Sync;

[MethodLogging]
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
    private readonly ILogService _logService;

    private CancellationTokenSource? _cts;
    private Task? _worker;
    private Guid _userId;
    private bool _autoSyncRunning = false;
    
    public bool IsRunning => _worker != null;
    

    public AutoSyncService(
        IApplicationSettingsService settings,
        IEnumerable<IEntitySynchroniser> synchronisers,
        INetworkService networkService,
        IAuthenticationService authenticationService,
        SQLiteContext context,
        ISyncService syncService,
        ISyncStateService syncStateService,
        ISyncStatusService syncStatusService,
        ILogService logService)
    {
        _settings = settings;
        _synchronisers = synchronisers;
        _networkService = networkService;
        _authenticationService = authenticationService;
        _context = context;
        _syncService = syncService;
        _syncStateService = syncStateService;
        _syncStatusService = syncStatusService;
        _logService = logService;
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
                if (!_autoSyncRunning)
                {
                    _autoSyncRunning = true;

                    MethodLoggingService.Write($"  AutoSyncing Start");
                    bool hasLocalChanges = await _syncStateService.HasLocalChangesAsync(token);
                    bool hasRemoteChanges = await _syncStateService.HasRemoteChangesAsync(_userId, token);
                    if (hasLocalChanges || hasRemoteChanges)
                    {
                        await _syncService.SyncAsync(_userId, hasLocalChanges, hasRemoteChanges, token);

                        await _syncStateService.UpdateRemoteSyncVersionAsync(_userId, token);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(2), token);
                    MethodLoggingService.Write($"  AutoSyncing End");
                }
            }
            finally
            {
                _autoSyncRunning = false;
            }
            await Task.Delay(TimeSpan.FromSeconds(5), token);
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
        {
            MethodLoggingService.Write($"    - Not connected to network - skipping");
        }
        else
        {
            bool hasLocalChanges = await _syncStateService.HasLocalChangesAsync(cancellationToken);
            bool hasRemoteChanges = await _syncStateService.HasRemoteChangesAsync(_userId, cancellationToken);

            MethodLoggingService.Write($"    - hasLocalChanges {hasLocalChanges}, hasRemoteChanges {hasRemoteChanges}");

            if (hasLocalChanges || hasRemoteChanges)
            {
                MethodLoggingService.Write($"    - calling _syncService.SyncAsync for current user");

                await _syncService.SyncAsync(_userId, hasLocalChanges, hasRemoteChanges, cancellationToken);

                await _syncStateService.UpdateRemoteSyncVersionAsync(_userId, cancellationToken);
            }
            else
            {
                MethodLoggingService.Write($"    - No changes - skipping");
            }

        }

        return true;
    }

}