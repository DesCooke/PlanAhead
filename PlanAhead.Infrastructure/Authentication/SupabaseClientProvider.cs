using Supabase;

namespace PlanAhead.Infrastructure.Authentication;

public class SupabaseClientProvider : ISupabaseClientProvider
{
    private readonly Client _client;
    private bool _initialised;

    public SupabaseClientProvider()
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false
        };

        _client = new Client(
            SupabaseSettings.Url,
            SupabaseSettings.PublishableKey,
            options);
    }

    public async Task<Client> GetClientAsync()
    {
        if (!_initialised)
        {
            await _client.InitializeAsync();
            _initialised = true;
        }

        return _client;
    }
}