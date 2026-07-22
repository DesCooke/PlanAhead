using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Services.Ledger;

public class LedgerService : ILedgerService
{
    private readonly ILedgerEntryRepository _repository;

    public LedgerService(
        ILedgerEntryRepository repository)
    {
        _repository = repository;
    }

    public Task<List<LedgerEntry>> GetAllAsync()
        => _repository.GetAllAsync();

    public Task<List<LedgerEntry>> GetByAccountIdAsync(
        Guid accountId)
        => _repository.GetByAccountIdAsync(accountId);

    public Task<List<LedgerEntry>> GetByFundIdAsync(
        Guid fundId)
        => _repository.GetByFundIdAsync(fundId);

    public Task<LedgerEntry?> GetByIdAsync(Guid id)
        => _repository.GetByIdAsync(id);

    public Task AddAsync(LedgerEntry ledgerEntry)
        => _repository.AddAsync(ledgerEntry);

    public Task UpdateAsync(LedgerEntry ledgerEntry)
        => _repository.UpdateAsync(ledgerEntry);

    public async Task DeleteAsync(Guid ledgerEntryId)
    {
        var ledgerEntry =
            await _repository.GetByIdAsync(ledgerEntryId);

        if (ledgerEntry != null)
            await _repository.DeleteAsync(ledgerEntry);
    }
}