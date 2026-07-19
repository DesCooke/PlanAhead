using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Data.Database;
using SQLite;

namespace PlanAhead.Repositories;

public class FundingRuleRepository : IFundingRuleRepository
{
    private readonly SQLiteContext _context;

    public FundingRuleRepository(SQLiteContext context)
    {
        _context = context;
    }

    private async Task<SQLiteAsyncConnection> Database()
    {
        var db = await _context.GetConnectionAsync();

        await db.CreateTableAsync<FundingRule>();

        return db;
    }

    public async Task<FundingRule?> GetByIdAsync(Guid id)
    {
        var db = await Database();

        return await db.Table<FundingRule>()
            .FirstOrDefaultAsync(r => r.Id == id && !r.Deleted);
    }

    public async Task<List<FundingRule>> GetByAccountIdAsync(Guid accountId)
    {
        var db = await Database();

        return await db.Table<FundingRule>()
            .Where(r => r.AccountId == accountId && !r.Deleted)
            .OrderBy(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<List<FundingRule>> GetByFundIdAsync(Guid fundId)
    {
        var db = await Database();

        return await db.Table<FundingRule>()
            .Where(r => r.FundId == fundId && !r.Deleted)
            .OrderBy(r => r.StartDate)
            .ToListAsync();
    }

    public async Task AddAsync(FundingRule rule)
    {
        var db = await Database();

        rule.Id = Guid.NewGuid();
        rule.CreatedUtc = DateTime.UtcNow;
        rule.UpdatedUtc = DateTime.UtcNow;
        rule.NeedsSync = true;

        await db.InsertAsync(rule);
    }

    public async Task UpdateAsync(FundingRule rule)
    {
        var db = await Database();

        rule.UpdatedUtc = DateTime.UtcNow;
        rule.NeedsSync = true;

        await db.UpdateAsync(rule);
    }

    public async Task DeleteAsync(FundingRule rule)
    {
        rule.Deleted = true;

        await UpdateAsync(rule);
    }
}