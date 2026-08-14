using PlanAhead.Core.Interfaces.Services;
using System.Diagnostics;


namespace PlanAhead.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISupabaseClientProvider _provider;

    public AuthenticationService(
        ISupabaseClientProvider provider)
    {
        _provider = provider;
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
}