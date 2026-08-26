using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using System.Security.Principal;

namespace PlanAhead.Core.Services.Funds;

public class FundService : IFundService
{
    private readonly IFundRepository _repository;

    public FundService(
        IFundRepository fundRepository)
    {
        _repository = fundRepository;
    }

    public Task<List<Fund>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<List<Fund>> GetByAccountIdAsync(
        Guid accountId)
    {
        return _repository.GetByAccountIdAsync(accountId);
    }

    public Task<Fund?> GetByIdAsync(
        Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task AddAsync(
        Fund fund)
    {
        // 
        // we set these now in the business logic because at this point
        // we are actually adding a new row
        // But _repository.AddAsync is also called when we 
        // add a row for synchronisation - in that circumstance - we do not
        // set these variables - that is why we set them here
        //
        fund.CreatedUtc = DateTime.UtcNow;
        fund.UpdatedUtc = DateTime.UtcNow;
        fund.NeedsSync = true;

        return _repository.AddAsync(fund);
    }

    public Task UpdateAsync(
        Fund fund)
    {
        //
        // We do this now rather than in the FundRepository because
        // here, we know we are updating the record
        // FundRepostiry.UpdateAsync gets called for synchronisation
        // updates also - where we do not want to update these values
        //
        fund.UpdatedUtc = DateTime.UtcNow;
        fund.NeedsSync = true;


        return _repository.UpdateAsync(fund);
    }

    public async Task DeleteAsync(
        Fund fund)
    {
        fund.Deleted = true;
        fund.DeletedUtc = DateTime.UtcNow;
        fund.NeedsSync = true;

        await _repository.DeleteAsync(fund);
    }
}