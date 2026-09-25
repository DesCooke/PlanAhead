using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Logging;
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
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await _context.GetConnectionAsync();

            await db.CreateTableAsync<Fund>();

            return db;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<List<Fund>> GetAllAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<Fund>()
                .Where(f => !f.Deleted)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<Fund?> GetByIdAsync(Guid id)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<Fund>()
                .FirstOrDefaultAsync(f => f.Id == id && !f.Deleted);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<List<Fund>> GetByAccountIdAsync(Guid accountId)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<Fund>()
                .Where(f => f.AccountId == accountId && !f.Deleted)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task AddAsync(Fund fund)
    {
        using var log = MethodLoggingService.Begin();
        try
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
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task UpdateAsync(Fund fund)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            await db.UpdateAsync(fund);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<List<Fund>> GetPendingSyncAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<Fund>()
                .Where(a => a.NeedsSync)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task MarkSyncedAsync(Guid id)
    {
        using var log = MethodLoggingService.Begin();
        try
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
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
    public async Task DeleteAsync(Fund fund)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            await db.UpdateAsync(fund);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
}