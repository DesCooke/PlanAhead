using Supabase;

namespace PlanAhead.Infrastructure.Authentication;

public interface ISupabaseClientProvider
{
    Task<Client> GetClientAsync();
}