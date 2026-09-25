using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Logging;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Logging;
using PlanAhead.Infrastructure.Sync.Models;
using Supabase;

namespace PlanAhead.Infrastructure.Sync;

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
        using var log = MethodLoggingService.Begin();
        try
        {
            var remoteVersion =
            await GetRemoteSyncVersionAsync(userId, cancellationToken);

            _settings.LastRemoteSyncVersion = remoteVersion;
            await _logService.LogAsync($"_settings.LastRemoteSyncVersion now {_settings.LastRemoteSyncVersion}");
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<bool> HasRemoteChangesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var remoteVersion =
            await GetRemoteSyncVersionAsync(userId, cancellationToken);

            await _logService.LogAsync($"HasRemoteChangesAsync, remoteVersion {remoteVersion}, Local {_settings.LastRemoteSyncVersion}");

            return remoteVersion > _settings.LastRemoteSyncVersion;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<long> GetRemoteSyncVersionAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var response = await _client
            .From<UserSyncRecord>()
            .Where(x => x.UserId == userId)
            .Single();

            if (response == null)
                return 0;

            await _logService.LogAsync($"GetRemoteSyncVersionAsync returns response.SyncVersion");

            return response.SyncVersion;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task UpdateLocalSyncVersionAsync(CancellationToken cancellationToken = default)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var localVersion =
            await GetLocalSyncVersionAsync(cancellationToken);

            _settings.LastLocalSyncVersion = localVersion;

            await _logService.LogAsync($"_settings.LastLocalSyncVersion is not {_settings.LastLocalSyncVersion}");

        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<bool> HasLocalChangesAsync(CancellationToken cancellationToken = default)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var localVersion =
            await GetLocalSyncVersionAsync(cancellationToken);

            await _logService.LogAsync($"HasLocalChangesAsync, localVersion {localVersion}, _settings.LastLocalSyncVersion {_settings.LastLocalSyncVersion}");

            return localVersion > _settings.LastLocalSyncVersion;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<long> GetLocalSyncVersionAsync(CancellationToken cancellationToken = default)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            await _logService.LogAsync($"GetLocalSyncVersionAsync returns _settings.LastLocalVersion");
            return _settings.LastLocalVersion;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task IncreaseLocalVersion(CancellationToken cancellationToken = default)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            _settings.LastLocalVersion++;
            await _logService.LogAsync($"IncreaseLocalVersion, _settings.LastLocalVersion is now {_settings.LastLocalVersion}");
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

}