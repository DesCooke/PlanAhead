using Supabase;

namespace PlanAhead.Infrastructure.Authentication;

public class SupabaseClientProvider : ISupabaseClientProvider
{
    private readonly Client _client;
    private bool _initialised;

    public SupabaseClientProvider(Client client)
    {
        _client = client;
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