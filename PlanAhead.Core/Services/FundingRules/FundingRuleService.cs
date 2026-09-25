using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Services.FundingRules;

public class FundingRuleService : IFundingRuleService
{
    private readonly IFundingRuleRepository _repository;

    public FundingRuleService(
        IFundingRuleRepository repository)
    {
        _repository = repository;
    }

    public Task<List<FundingRule>> GetAllAsync()
        => _repository.GetAllAsync();

    public Task<List<FundingRule>> GetByFundIdAsync(Guid fundId)
        => _repository.GetByFundIdAsync(fundId);

    public Task<FundingRule?> GetByIdAsync(Guid id)
        => _repository.GetByIdAsync(id);

    public Task<FundingRule?> GetByFundAndPeriodAsync(
        Guid fundId,
        DateOnly periodStart)
        => _repository.GetByFundAndPeriodAsync(
            fundId,
            periodStart);

    public Task AddAsync(FundingRule fundingRule)
        => _repository.AddAsync(fundingRule);

    public Task UpdateAsync(FundingRule fundingRule)
        => _repository.UpdateAsync(fundingRule);

    public async Task DeleteAsync(Guid fundingRuleId)
    {
        var rule =
            await _repository.GetByIdAsync(fundingRuleId);

        if (rule != null)
            await _repository.DeleteAsync(rule);
    }
}