using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.MethodLogging;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Models.Sync;
using PlanAhead.Infrastructure.DB.SQLite;
using PlanAhead.Infrastructure.Logging;
using SQLite;
using Supabase.Postgrest.Models;
using System.Security.Principal;
using System.Text.Json;

namespace PlanAhead.Infrastructure.Repositories;

[MethodLogging]
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

        var retAccounts = await db.Table<Account>()
                .Where(a => a.NeedsSync)
                .ToListAsync();

        foreach (var account in retAccounts)
        {
            MethodLoggingService.Write(
              $"{JsonSerializer.Serialize(account)}");
        }

        return retAccounts;

    }

    public async Task MarkSyncedAsync(Guid id)
    {
        var db = await _context.GetConnectionAsync();

        var account = await GetByIdAsync(id);
        if (account != null)
        {
            account.NeedsSync = false;

            MethodLoggingService.Write(
              $"Marking {id} as NeedsSync = false");

            await db.UpdateAsync(account);
        }
    }
    public async Task<List<Account>> GetAllAsync()
    {
        var db = await Database();

        var retAccounts = await db.Table<Account>()
            .Where(a => !a.Deleted)
            .OrderBy(a => a.DisplayOrder)
            .ToListAsync();

        foreach (var account in retAccounts)
        {
            MethodLoggingService.Write(
              $"{JsonSerializer.Serialize(account)}");
        }

        return retAccounts;

    }

    public async Task UpsertAsync(Account account)
    {
        var db = await Database();

        MethodLoggingService.Write(
          $"UpsertAccount Account: {JsonSerializer.Serialize(account)}");

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

        var retAccounts = await db.Table<Account>()
            .Where(a => !a.Deleted && !a.Archived)
            .OrderBy(a => a.DisplayOrder)
            .ToListAsync();

        foreach (var account in retAccounts)
        {
            MethodLoggingService.Write(
              $"{JsonSerializer.Serialize(account)}");
        }

        return retAccounts;
    }
    public async Task<Account?> GetByIdAsync(Guid id)
    {
        var db = await Database();

        var retAccount = await db.Table<Account>()
                       .FirstOrDefaultAsync(a => a.Id == id &&
            !a.Deleted);

        MethodLoggingService.Write(
          $"Account {id}: {JsonSerializer.Serialize(retAccount)}");

        return retAccount;
    }

    public async Task AddAsync(Account account)
    {
        var db = await Database();

        if (account.Id == Guid.Empty)
        {
            account.Id = Guid.NewGuid();
            account.DisplayOrder = await db.Table<Account>().CountAsync() + 1;
        }

        MethodLoggingService.Write(
            $"Adding Account: {JsonSerializer.Serialize(account)}");

        await db.InsertAsync(account);

    }

    public async Task UpdateAsync(Account account)
    {
        var db = await Database();

        MethodLoggingService.Write(
            $"Updating Account: {JsonSerializer.Serialize(account)}");

        await db.UpdateAsync(account);

    }

    public async Task DeleteAsync(Account account)
    {
        var db = await Database();

        MethodLoggingService.Write(
            $"Marking Account as Deleted: {JsonSerializer.Serialize(account)}");

        await db.UpdateAsync(account);
    }
}