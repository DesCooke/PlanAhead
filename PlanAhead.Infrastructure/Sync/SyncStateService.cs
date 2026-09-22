using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Logging;
using PlanAhead.Infrastructure.Sync.Models;
using Supabase;
using PlanAhead.Core.MethodLogging;

namespace PlanAhead.Infrastructure.Sync;

[MethodLogging]
public class SyncStateService : ISyncStateService
{
    private readonly Client _client;
    private readonly IApplicationSettingsService _settings;
    private readonly ILogService _logService;

    public SyncStateService(
        Client client,
        IAuthenticationService authenticationService,
        IApplicationSettingsService settings,
        ILogService logService)
    {
        _client = client;
        _settings = settings;
        _logService = logService;
    }


    public async Task UpdateRemoteSyncVersionAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var remoteVersion =
            await GetRemoteSyncVersionAsync(userId, cancellationToken);

        _settings.LastRemoteSyncVersion = remoteVersion;
        MethodLoggingService.Write($"  _settings.LastRemoteSyncVersion now {_settings.LastRemoteSyncVersion}");
    }

    public async Task<bool> HasRemoteChangesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var remoteVersion =
            await GetRemoteSyncVersionAsync(userId, cancellationToken);

        MethodLoggingService.Write($"  HasRemoteChangesAsync, remoteVersion {remoteVersion}, Local {_settings.LastRemoteSyncVersion}");

        return remoteVersion > _settings.LastRemoteSyncVersion;
    }

    public async Task<long> GetRemoteSyncVersionAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var response = await _client
            .From<UserSyncRecord>()
            .Where(x => x.UserId == userId)
            .Single();

        if (response == null)
            return 0L;

        MethodLoggingService.Write($"  GetRemoteSyncVersionAsync returns response.SyncVersion");

        return response.SyncVersion;
    }

    public async Task UpdateLocalSyncVersionAsync(CancellationToken cancellationToken = default)
    {
        var localVersion =
            await GetLocalSyncVersionAsync(cancellationToken);

        _settings.LastLocalSyncVersion = localVersion;

        MethodLoggingService.Write($"  _settings.LastLocalSyncVersion is not {_settings.LastLocalSyncVersion}");

    }

    public async Task<bool> HasLocalChangesAsync(CancellationToken cancellationToken = default)
    {
        var localVersion =
            await GetLocalSyncVersionAsync(cancellationToken);

        MethodLoggingService.Write($"  HasLocalChangesAsync, localVersion {localVersion}, _settings.LastLocalSyncVersion {_settings.LastLocalSyncVersion}");

        return localVersion > _settings.LastLocalSyncVersion;
    }

    public async Task<long> GetLocalSyncVersionAsync(CancellationToken cancellationToken = default)
    {
        MethodLoggingService.Write($"  GetLocalSyncVersionAsync returns _settings.LastLocalVersion");
        return _settings.LastLocalVersion;
    }

    public async Task IncreaseLocalVersion(CancellationToken cancellationToken = default)
    {
        
        _settings.LastLocalVersion++;
        MethodLoggingService.Write($"  IncreaseLocalVersion, _settings.LastLocalVersion is now {_settings.LastLocalVersion}");
    }

}