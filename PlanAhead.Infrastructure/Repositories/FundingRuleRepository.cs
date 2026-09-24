using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Logging;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Infrastructure.DB.SQLite;
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
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await _context.GetConnectionAsync();

            await db.CreateTableAsync<FundingRule>();

            return db;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<FundingRule?> GetByIdAsync(Guid id)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<FundingRule>()
                .FirstOrDefaultAsync(r =>
                    r.Id == id &&
                    !r.Deleted);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<List<FundingRule>> GetAllAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<FundingRule>()
                .Where(r => !r.Deleted)
                .OrderBy(r => r.FundId)
                .ThenBy(r => r.StartDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<List<FundingRule>> GetByFundIdAsync(Guid fundId)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<FundingRule>()
                .Where(r =>
                    r.FundId == fundId &&
                    !r.Deleted)
                .OrderBy(r => r.StartDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<FundingRule?> GetByFundAndPeriodAsync(
        Guid fundId,
        DateOnly periodStart)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<FundingRule>()
                .FirstOrDefaultAsync(r =>
                    r.FundId == fundId &&
                    r.StartDate == periodStart &&
                    !r.Deleted);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task AddAsync(FundingRule fundingRule)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            fundingRule.Id = Guid.NewGuid();
            fundingRule.CreatedUtc = DateTime.UtcNow;
            fundingRule.UpdatedUtc = DateTime.UtcNow;
            fundingRule.NeedsSync = true;

            await db.InsertAsync(fundingRule);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task UpdateAsync(FundingRule fundingRule)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            fundingRule.UpdatedUtc = DateTime.UtcNow;
            fundingRule.NeedsSync = true;

            await db.UpdateAsync(fundingRule);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task DeleteAsync(FundingRule fundingRule)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            fundingRule.Deleted = true;

            await UpdateAsync(fundingRule);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
}