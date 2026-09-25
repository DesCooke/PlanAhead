using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Logging;
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
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await _context.GetConnectionAsync();
            return await db.Table<Account>()
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
            var db = await _context.GetConnectionAsync();

            var account = await GetByIdAsync(id);
            if (account != null)
            {
                account.NeedsSync = false;

                await db.UpdateAsync(account);
            }
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
    public async Task<List<Account>> GetAllAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<Account>()
                .Where(a => !a.Deleted)
                .OrderBy(a => a.DisplayOrder)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task UpsertAsync(Account account)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            await db.InsertOrReplaceAsync(account);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    private async Task<SQLiteAsyncConnection> Database()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await _context.GetConnectionAsync();

            await db.CreateTableAsync<Account>();

            return db;
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task<List<Account>> GetActiveAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<Account>()
                .Where(a => !a.Deleted && !a.Archived)
                .OrderBy(a => a.DisplayOrder)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
    public async Task<Account?> GetByIdAsync(Guid id)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            return await db.Table<Account>()
                           .FirstOrDefaultAsync(a => a.Id == id &&
                !a.Deleted);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task AddAsync(Account account)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            if (account.Id == Guid.Empty)
            {
                account.Id = Guid.NewGuid();
                account.DisplayOrder = await db.Table<Account>().CountAsync() + 1;
            }

            await db.InsertAsync(account);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task UpdateAsync(Account account)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            await db.UpdateAsync(account);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public async Task DeleteAsync(Account account)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            var db = await Database();

            await db.UpdateAsync(account);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
}