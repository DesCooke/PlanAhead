namespace PlanAhead.Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(
        string email,
        string password);

    Task<bool> RegisterAsync(
        string email,
        string password);

    Task LogoutAsync();

    Task<bool> IsLoggedInAsync();

    Task<string?> GetCurrentUserIdAsync();
}