using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Interfaces.Repositories;

public interface ILedgerEntryRepository
{
    Task<LedgerEntry?> GetByIdAsync(Guid id);

    Task<List<LedgerEntry>> GetAllAsync();

    Task<List<LedgerEntry>> GetByAccountIdAsync(Guid accountId);

    Task<List<LedgerEntry>> GetByFundIdAsync(Guid fundId);

    Task<List<LedgerEntry>> GetBetweenDatesAsync(
        Guid accountId,
        DateOnly from,
        DateOnly to);

    Task AddAsync(LedgerEntry ledgerEntry);

    Task UpdateAsync(LedgerEntry ledgerEntry);

    Task DeleteAsync(LedgerEntry ledgerEntry);
}