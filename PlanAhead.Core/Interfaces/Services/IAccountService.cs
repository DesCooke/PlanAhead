using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Interfaces.Services;

public interface IAccountService
{
    Task<List<Account>> GetAllAsync();

    Task<Account?> GetByIdAsync(Guid id);

    Task AddAsync(Account account);

    Task UpdateAsync(Account account);

    Task DeleteAsync(Account accou);
}