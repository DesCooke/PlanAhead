using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Sync;
using Supabase;
using Supabase.Gotrue;
using System.Diagnostics;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;


namespace PlanAhead.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISupabaseClientProvider _provider;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ISyncService _syncService;

    public AuthenticationService(
        ISupabaseClientProvider provider, ISecureStorageService secureStorageService, 
        ISyncService syncService)
    {
        _provider = provider;
        _secureStorageService = secureStorageService;
        _syncService = syncService;
    }

    public async Task<Supabase.Gotrue.Session?> LoginAsync(
        string email,
        string password)
    {
        var client = await _provider.GetClientAsync();

        var session = await client.Auth.SignIn(email, password);

        if (session != null)
        {
            await _secureStorageService.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(session));
        }

        return session;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var client = await _provider.GetClientAsync();
        return client.Auth.CurrentUser;
    }

    public async Task LogoutAsync()
    {
        var client = await _provider.GetClientAsync();

        await client.Auth.SignOut();

        _secureStorageService.Remove("supabase-session");
    }

    public async Task<bool> RegisterAsync(
        string email,
        string password)
    {
        var client = await _provider.GetClientAsync();

        var session = await client.Auth.SignUp(email, password);

        if (session != null)
        {
            await _secureStorageService.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(session));
        }

        return session?.User != null;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var client = await _provider.GetClientAsync();

        Debug.WriteLine("===== IsLoggedInAsync =====");
        Debug.WriteLine($"CurrentUser    : {client.Auth.CurrentUser?.Email}");
        Debug.WriteLine($"CurrentSession : {client.Auth.CurrentSession != null}");

        return client.Auth.CurrentUser != null;
    }

    public async Task<string?> GetCurrentUserIdAsync()
    {
        var client = await _provider.GetClientAsync();

        return client.Auth.CurrentUser?.Id;
    }

    public async Task<string?> GetCurrentUserEmailAsync()
    {
        var client = await _provider.GetClientAsync();

        return client.Auth.CurrentUser?.Email;
    }

    public async Task<bool> RestoreSessionAsync()
    {
        var json = await _secureStorageService.GetAsync("supabase-session");

        if (string.IsNullOrWhiteSpace(json))
            return false;

        var session = JsonSerializer.Deserialize<Session>(json);

        if (session == null || session.AccessToken==null || session.RefreshToken==null)
            return false;

        var client = await _provider.GetClientAsync();

        try
        {
            var newSession = await client.Auth.SetSession(
                session.AccessToken,
                session.RefreshToken);

            await _secureStorageService.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(newSession));

            var userIdString = client.Auth.CurrentUser?.Id;
            if (userIdString != null)
            {
                var userId = Guid.Parse(userIdString);
                if(userId!=Guid.Empty)
                    await _syncService.SyncAsync(userId);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            _secureStorageService.Remove("supabase-session");
            return false;
        }
        return true;
    }

}