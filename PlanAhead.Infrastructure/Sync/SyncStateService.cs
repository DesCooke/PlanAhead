using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Sync.Models;
using Supabase;

namespace PlanAhead.Infrastructure.Sync;

public class SyncStateService : ISyncStateService
{
    private readonly Client _client;
    private readonly IApplicationSettingsService _settings;

    public SyncStateService(
        Client client,
        IAuthenticationService authenticationService,
        IApplicationSettingsService settings)
    {
        _client = client;
        _settings = settings;
    }


    public async Task MarkAsUptodateAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var remoteVersion =
            await GetRemoteSyncVersionAsync(userId, cancellationToken);

        _settings.LastSyncVersion = remoteVersion;
    }

    public async Task<bool> HasRemoteChangesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var remoteVersion =
            await GetRemoteSyncVersionAsync(userId, cancellationToken);

        return remoteVersion > _settings.LastSyncVersion;
    }

    public async Task<long> GetRemoteSyncVersionAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var response = await _client
            .From<UserSyncRecord>()
            .Where(x => x.UserId == userId)
            .Single();

        if (response == null)
            return 0;

        return response.SyncVersion;
    }
}