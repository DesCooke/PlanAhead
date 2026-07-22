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
        // Future business rules go here.

        return _fundRepository.AddAsync(fund);
    }

    public Task UpdateAsync(
        Fund fund)
    {
        // Validation will eventually live here.

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