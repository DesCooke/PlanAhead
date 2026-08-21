using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Services.Funds;

public class FundService : IFundService
{
    private readonly IFundRepository _fundRepository;

    public FundService(
        IFundRepository fundRepository)
    {
        _fundRepository = fundRepository;
    }

    public Task<List<Fund>> GetAllAsync()
    {
        return _fundRepository.GetAllAsync();
    }

    public Task<List<Fund>> GetByAccountIdAsync(
        Guid accountId)
    {
        return _fundRepository.GetByAccountIdAsync(accountId);
    }

    public Task<Fund?> GetByIdAsync(
        Guid id)
    {
        return _fundRepository.GetByIdAsync(id);
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

        return _fundRepository.AddAsync(fund);
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


        return _fundRepository.UpdateAsync(fund);
    }

    public async Task DeleteAsync(
        Guid fundId)
    {
        var fund =
            await _fundRepository.GetByIdAsync(fundId);

        if (fund == null)
            return;

        await _fundRepository.DeleteAsync(fund);
    }
}