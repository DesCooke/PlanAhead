using PlanAhead.Core.MethodLogging;
using PlanAhead.Core.Models.Sync;
using PlanAhead.Infrastructure.DB.SQLite;

namespace PlanAhead.Data.Database;

[MethodLogging]
public class DatabaseStartup
{
    private readonly SQLiteContext _context;

    public DatabaseStartup(SQLiteContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
    }
}