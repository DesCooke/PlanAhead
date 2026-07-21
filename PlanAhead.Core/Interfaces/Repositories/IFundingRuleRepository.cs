using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Interfaces.Repositories;

public interface IFundingRuleRepository
{
    Task<FundingRule?> GetByIdAsync(Guid id);

    Task<List<FundingRule>> GetAllAsync();

    Task<List<FundingRule>> GetByFundIdAsync(Guid fundId);

    Task<FundingRule?> GetByFundAndPeriodAsync(
        Guid fundId,
        DateOnly periodStart);

    Task AddAsync(FundingRule fundingRule);

    Task UpdateAsync(FundingRule fundingRule);

    Task DeleteAsync(FundingRule fundingRule);
}