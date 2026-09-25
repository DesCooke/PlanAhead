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
using PlanAhead.Core.Logging;

namespace PlanAhead.Infrastructure.Authentication;

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
        using var log = MethodLoggingService.Begin();
        try
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
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var client = await _provider.GetClientAsync();
            return client.Auth.CurrentUser;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var client = await _provider.GetClientAsync();

            await client.Auth.SignOut();

            _secureStorageService.Remove("supabase-session");
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<bool> RegisterAsync(
        string email,
        string password)
    {
        using var log = MethodLoggingService.Begin();
        try
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
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<bool> IsLoggedInAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var client = await _provider.GetClientAsync();

            log.Log($"CurrentUser    : {client.Auth.CurrentUser?.Email}");
            log.Log($"CurrentSession : {client.Auth.CurrentSession != null}");

            return client.Auth.CurrentUser != null;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<string?> GetCurrentUserIdAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var client = await _provider.GetClientAsync();

            return client.Auth.CurrentUser?.Id;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<string?> GetCurrentUserEmailAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var client = await _provider.GetClientAsync();

            return client.Auth.CurrentUser?.Email;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task EnsureSessionAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
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
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<bool> RestoreSessionAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var json = await _secureStorageService.GetAsync("supabase-session");

            if (string.IsNullOrWhiteSpace(json))
                return false;

            var session = JsonSerializer.Deserialize<Session>(json);

            if (session == null || session.AccessToken == null || session.RefreshToken == null)
                return false;

            var client = await _provider.GetClientAsync();

            var newSession = await client.Auth.SetSession(
                session.AccessToken,
                session.RefreshToken);

            await _secureStorageService.SetAsync(
                "supabase-session",
                JsonSerializer.Serialize(newSession));
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
        return true;
    }

}