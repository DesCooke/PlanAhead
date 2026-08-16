using PlanAhead.Core.Interfaces.Services;
using Supabase.Gotrue;
using System.Diagnostics;
using System.Text.Json;

using PlanAhead.Infrastructure.Authentication;
using Supabase;


namespace PlanAhead.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISupabaseClientProvider _provider;
    private readonly ISecureStorageService _secureStorageService;

    public AuthenticationService(
        ISupabaseClientProvider provider, ISecureStorageService secureStorageService)
    {
        _provider = provider;
        _secureStorageService = secureStorageService;
    }

    public async Task<Supabase.Gotrue.Session> LoginAsync(
        string email,
        string password)
    {
        var client = await _provider.GetClientAsync();

        var response = await client.Auth.SignIn(email, password);


        return response;
    }

    public async Task LogoutAsync()
    {
        var client = await _provider.GetClientAsync();

        await client.Auth.SignOut();
    }

    public async Task<bool> RegisterAsync(
        string email,
        string password)
    {
        var client = await _provider.GetClientAsync();

        var response = await client.Auth.SignUp(email, password);

        return response?.User != null;
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

        if (session == null)
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

            return true;
        }
        catch
        {
            _secureStorageService.Remove("supabase-session");
            return false;
        }
    }

}