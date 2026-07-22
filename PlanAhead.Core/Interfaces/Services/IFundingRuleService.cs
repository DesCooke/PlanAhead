using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Interfaces.Services;

public interface IFundingRuleService
{
    Task<List<FundingRule>> GetAllAsync();

    Task<List<FundingRule>> GetByFundIdAsync(Guid fundId);

    Task<FundingRule?> GetByIdAsync(Guid id);

    Task<FundingRule?> GetByFundAndPeriodAsync(
        Guid fundId,
        DateOnly periodStart);

    Task AddAsync(FundingRule fundingRule);

    Task UpdateAsync(FundingRule fundingRule);

    Task DeleteAsync(Guid fundingRuleId);
}