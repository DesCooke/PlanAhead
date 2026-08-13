using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Infrastructure.Database.SQLite;
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

        fund.Id = Guid.NewGuid();
        fund.CreatedUtc = DateTime.UtcNow;
        fund.UpdatedUtc = DateTime.UtcNow;
        fund.NeedsSync = true;

        // Put new funds at the end of the list
        fund.DisplayOrder =
            await db.Table<Fund>()
                    .Where(f => f.AccountId == fund.AccountId && !f.Deleted)
                    .CountAsync() + 1;

        await db.InsertAsync(fund);
    }

    public async Task UpdateAsync(Fund fund)
    {
        var db = await Database();

        fund.UpdatedUtc = DateTime.UtcNow;
        fund.NeedsSync = true;

        await db.UpdateAsync(fund);
    }

    public async Task DeleteAsync(Fund fund)
    {
        fund.Deleted = true;

        await UpdateAsync(fund);
    }
}