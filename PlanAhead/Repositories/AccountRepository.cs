using PlanAhead.Data.Database;
using PlanAhead.Core.Models.Domain;
using SQLite;

namespace PlanAhead.Repositories;

public class AccountRepository
{
    private readonly SQLiteContext _context;

    public AccountRepository(SQLiteContext context)
    {
        _context = context;
    }

    private async Task<SQLiteAsyncConnection> Database()
    {
        var db = await _context.GetConnectionAsync();

        await db.CreateTableAsync<Account>();

        return db;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        var db = await Database();

        return await db.Table<Account>()
                       .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Account>> GetAllAsync()
    {
        var db = await Database();

        return await db.Table<Account>()
                       .Where(a => !a.Deleted)
                       .OrderBy(a => a.DisplayOrder)
                       .ToListAsync();
    }

    public async Task AddAsync(Account account)
    {
        var db = await Database();

        account.Id = Guid.NewGuid();

        account.CreatedUtc = DateTime.UtcNow;

        account.UpdatedUtc = DateTime.UtcNow;

        account.NeedsSync = true;

        account.DisplayOrder = await db.Table<Account>().CountAsync() + 1;

        await db.InsertAsync(account);
    }

    public async Task UpdateAsync(Account account)
    {
        var db = await Database();

        account.UpdatedUtc = DateTime.UtcNow;

        account.NeedsSync = true;

        await db.UpdateAsync(account);
    }

    public async Task DeleteAsync(Account account)
    {
        account.Deleted = true;

        await UpdateAsync(account);
    }
}