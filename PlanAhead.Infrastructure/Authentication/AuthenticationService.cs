using Newtonsoft.Json.Linq;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Logging;
using PlanAhead.Infrastructure.Sync;
using Supabase;
using Supabase.Gotrue;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;


namespace PlanAhead.Infrastructure.Authentication;

[MethodLogging]
public class AuthenticationService : IAuthenticationService
{
    private readonly ISupabaseClientProvider _provider;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ILogService _logService;

    public AuthenticationService(
        ISupabaseClientProvider provider, ISecureStorageService secureStorageService, 
        ILogService logService)
    {
        _provider = provider;
        _secureStorageService = secureStorageService;
        _logService = logService;
    }


    public async Task<Supabase.Gotrue.Session?> LoginAsync(
        string email,
        string password)
    {
        var client = await _provider.GetClientAsync();

        var session = await client.Auth.SignIn(email, password);
        if (session != null)
        {
            var json = JsonSerializer.Serialize(session);
            await _secureStorageService.SetAsync(
                "supabase-session",
                json);            
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

        MethodLoggingService.Write($"  CurrentUser    : {client.Auth.CurrentUser?.Email}");

        var retval = client.Auth.CurrentUser != null;

        MethodLoggingService.Write($"  retval : {retval}");

        return retval;
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

    public async Task EnsureSessionAsync()
    {
        var client = await _provider.GetClientAsync();

        var session = client.Auth.CurrentSession;

        if (session == null)
            throw new Exception("Not logged in.");

        var newSession = await client.Auth.SetSession(
            session.AccessToken!,
            session.RefreshToken!);

        await _secureStorageService.SetAsync(
            "supabase-session",
            JsonSerializer.Serialize(newSession));
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
        }
        catch (Exception ex)
        {
            _logService.LogException(ex);
            return false;
        }
        return true;
    }

}