using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Logging;

namespace PlanAhead.Core.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(
        IAccountRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Account>> GetAllAsync()
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            return _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public Task<Account?> GetByIdAsync(Guid id)
        => _repository.GetByIdAsync(id);

    public Task AddAsync(Account account)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            // 
            // we set these now in the business logic because at this point
            // we are actually adding a new row
            // But _repository.AddAsync is also called when we 
            // add a row for synchronisation - in that circumstance - we do not
            // set these variables - that is why we set them here
            //
            account.CreatedUtc = DateTime.UtcNow;
            account.UpdatedUtc = DateTime.UtcNow;
            account.NeedsSync = true;

            return _repository.AddAsync(account);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }

    public Task UpdateAsync(Account account)
    {
        using var log = MethodLoggingService.Begin();
        try
        {
            account.UpdatedUtc = DateTime.UtcNow;
            account.NeedsSync = true;

            return _repository.UpdateAsync(account);
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
            account.Deleted = true;
            account.DeletedUtc = DateTime.UtcNow;
            account.NeedsSync = true;

            await _repository.DeleteAsync(account);
        }
        catch (Exception ex)
        {
            log.Exception(ex);
            throw;
        }
    }
}