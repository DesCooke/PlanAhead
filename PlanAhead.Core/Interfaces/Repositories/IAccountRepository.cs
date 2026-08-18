using PlanAhead.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id);

        Task AddAsync(Account account);

        Task UpdateAsync(Account account);

        Task DeleteAsync(Account account);

        Task<List<Account>> GetAllAsync();

        Task<List<Account>> GetActiveAsync();

        Task UpsertAsync(Account account);

        Task<List<Account>> GetPendingSyncAsync();

        Task MarkSyncedAsync(Guid id);
    }
}
