using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;

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
        => _repository.GetAllAsync();

    public Task<Account?> GetByIdAsync(Guid id)
        => _repository.GetByIdAsync(id);

    public Task AddAsync(Account account)
        => _repository.AddAsync(account);

    public Task UpdateAsync(Account account)
        => _repository.UpdateAsync(account);

    public async Task DeleteAsync(Guid accountId)
    {
        var account =
            await _repository.GetByIdAsync(accountId);

        if (account != null)
            await _repository.DeleteAsync(account);
    }
}