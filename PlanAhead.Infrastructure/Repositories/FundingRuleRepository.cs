using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Infrastructure.Database.SQLite;
using SQLite;

namespace PlanAhead.Infrastructure.Repositories;

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
            .FirstOrDefaultAsync(r =>
                r.Id == id &&
                !r.Deleted);
    }

    public async Task<List<FundingRule>> GetAllAsync()
    {
        var db = await Database();

        return await db.Table<FundingRule>()
            .Where(r => !r.Deleted)
            .OrderBy(r => r.FundId)
            .ThenBy(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<List<FundingRule>> GetByFundIdAsync(Guid fundId)
    {
        var db = await Database();

        return await db.Table<FundingRule>()
            .Where(r =>
                r.FundId == fundId &&
                !r.Deleted)
            .OrderBy(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<FundingRule?> GetByFundAndPeriodAsync(
        Guid fundId,
        DateOnly periodStart)
    {
        var db = await Database();

        return await db.Table<FundingRule>()
            .FirstOrDefaultAsync(r =>
                r.FundId == fundId &&
                r.StartDate == periodStart &&
                !r.Deleted);
    }

    public async Task AddAsync(FundingRule fundingRule)
    {
        var db = await Database();

        fundingRule.Id = Guid.NewGuid();
        fundingRule.CreatedUtc = DateTime.UtcNow;
        fundingRule.UpdatedUtc = DateTime.UtcNow;
        fundingRule.NeedsSync = true;

        await db.InsertAsync(fundingRule);
    }

    public async Task UpdateAsync(FundingRule fundingRule)
    {
        var db = await Database();

        fundingRule.UpdatedUtc = DateTime.UtcNow;
        fundingRule.NeedsSync = true;

        await db.UpdateAsync(fundingRule);
    }

    public async Task DeleteAsync(FundingRule fundingRule)
    {
        fundingRule.Deleted = true;

        await UpdateAsync(fundingRule);
    }
}