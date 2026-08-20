using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Models.Sync;
using PlanAhead.Infrastructure.DB.SQLite;
using SQLite;
using Supabase.Postgrest.Models;

namespace PlanAhead.Infrastructure.Repositories;

public class AccountRepository: IAccountRepository
{
    private readonly SQLiteContext _context;
    private readonly IApplicationSettingsService _settings;


    public AccountRepository(SQLiteContext context,
        IApplicationSettingsService settings
        )
    {
        _context = context;
        _settings = settings;
    }

    public async Task<List<Account>> GetPendingSyncAsync()
    {
        var db = await _context.GetConnectionAsync();

        try
        {
            return await db.Table<Account>()
                .Where(a => a.NeedsSync)
                .ToListAsync();
        } catch 
        {

        }
         return new List<Account>();
    }

    public async Task MarkSyncedAsync(Guid id)
    {
        var db = await _context.GetConnectionAsync();

        var account = await GetByIdAsync(id);
        if (account != null)
        {
            account.NeedsSync = false;

            await db.UpdateAsync(account);
        }
    }
    public async Task<List<Account>> GetAllAsync()
    {
        var db = await Database();

        return await db.Table<Account>()
            .Where(a => !a.Deleted)
            .OrderBy(a => a.DisplayOrder)
            .ToListAsync();
    }

    public async Task UpsertAsync(Account account)
    {
        var db = await Database();

        await db.InsertOrReplaceAsync(account);
    }

    private async Task<SQLiteAsyncConnection> Database()
    {
        var db = await _context.GetConnectionAsync();

        await db.CreateTableAsync<Account>();

        return db;
    }

    public async Task<List<Account>> GetActiveAsync()
    {
        var db = await Database();

        return await db.Table<Account>()
            .Where(a => !a.Deleted && !a.Archived)
            .OrderBy(a => a.DisplayOrder)
            .ToListAsync();
    }
    public async Task<Account?> GetByIdAsync(Guid id)
    {
        var db = await Database();

        return await db.Table<Account>()
                       .FirstOrDefaultAsync(a => a.Id == id &&
            !a.Deleted);
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