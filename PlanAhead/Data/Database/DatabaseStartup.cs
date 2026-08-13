using PlanAhead.Infrastructure.Database.SQLite;

namespace PlanAhead.Data.Database;

public class DatabaseStartup
{
    private readonly SQLiteContext _context;

    public DatabaseStartup(SQLiteContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        await _context.GetConnectionAsync();
    }
}