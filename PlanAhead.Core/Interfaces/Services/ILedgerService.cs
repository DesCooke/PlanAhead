using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Interfaces.Services;

public interface ILedgerService
{
    Task<List<LedgerEntry>> GetAllAsync();

    Task<List<LedgerEntry>> GetByAccountIdAsync(Guid accountId);

    Task<List<LedgerEntry>> GetByFundIdAsync(Guid fundId);

    Task<LedgerEntry?> GetByIdAsync(Guid id);

    Task AddAsync(LedgerEntry ledgerEntry);

    Task UpdateAsync(LedgerEntry ledgerEntry);

    Task DeleteAsync(Guid ledgerEntryId);
}