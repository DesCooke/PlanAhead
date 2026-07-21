using PlanAhead.Infrastructure.Data.Database;

namespace PlanAhead.Data.Database;

public class ApplicationStartupService
{
    private readonly SQLiteContext _context;

    public ApplicationStartupService(SQLiteContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        await _context.GetConnectionAsync();
    }
}