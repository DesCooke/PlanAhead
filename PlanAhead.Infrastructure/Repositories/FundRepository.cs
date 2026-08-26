using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Infrastructure.DB.SQLite;
using SQLite;

namespace PlanAhead.Infrastructure.Repositories;

public class FundRepository : IFundRepository
{
    private readonly SQLiteContext _context;

    public FundRepository(SQLiteContext context)
    {
        _context = context;
    }

    private async Task<SQLiteAsyncConnection> Database()
    {
        var db = await _context.GetConnectionAsync();

        await db.CreateTableAsync<Fund>();

        return db;
    }

    public async Task<List<Fund>> GetAllAsync()
    {
        var db = await Database();

        return await db.Table<Fund>()
            .Where(f => !f.Deleted)
            .OrderBy(f => f.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Fund?> GetByIdAsync(Guid id)
    {
        var db = await Database();

        return await db.Table<Fund>()
            .FirstOrDefaultAsync(f => f.Id == id && !f.Deleted);
    }

    public async Task<List<Fund>> GetByAccountIdAsync(Guid accountId)
    {
        var db = await Database();

        return await db.Table<Fund>()
            .Where(f => f.AccountId == accountId && !f.Deleted)
            .OrderBy(f => f.DisplayOrder)
            .ToListAsync();
    }

    public async Task AddAsync(Fund fund)
    {
        var db = await Database();

        if (fund.Id == Guid.Empty)
        {
            fund.Id = Guid.NewGuid();

            // Put new funds at the end of the list - only for
            // actually new funds - not synchronised
            fund.DisplayOrder =
                await db.Table<Fund>()
                        .Where(f => f.AccountId == fund.AccountId && !f.Deleted)
                        .CountAsync() + 1;
        }

        await db.InsertAsync(fund);
    }

    public async Task UpdateAsync(Fund fund)
    {
        var db = await Database();

        await db.UpdateAsync(fund);
    }

    public async Task<List<Fund>> GetPendingSyncAsync()
    {
        var db = await Database();

        try { 
        return await db.Table<Fund>()
            .Where(a => a.NeedsSync)
            .ToListAsync();
        }
        catch
        {

        }
        return new List<Fund>();

    }

    public async Task MarkSyncedAsync(Guid id)
    {
        var db = await Database();
        if (db != null)
        {
            var fund = await GetByIdAsync(id);
            if (fund != null)
            {
                fund.NeedsSync = false;

                await db.UpdateAsync(fund);
            }
        }
    }
    public async Task DeleteAsync(Fund fund)
    {
        var db = await Database();

        await db.UpdateAsync(fund);
    }
}