using Supabase.Gotrue;

namespace PlanAhead.Infrastructure.Authentication;

public interface IAuthenticationService
{
    Task<Supabase.Gotrue.Session?> LoginAsync(
        string email,
        string password);

    Task<bool> RegisterAsync(
        string email,
        string password);

    Task LogoutAsync();

    Task<bool> IsLoggedInAsync();

    Task<string?> GetCurrentUserIdAsync();

    Task<string?> GetCurrentUserEmailAsync();

    Task<bool> RestoreSessionAsync();

    Task<User?> GetCurrentUserAsync();

    Task EnsureSessionAsync();

}