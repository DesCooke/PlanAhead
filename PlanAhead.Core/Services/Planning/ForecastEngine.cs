using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Services.Planning;

public class ForecastEngine : IForecastEngine
{
    private readonly IFundingRuleRepository _fundingRuleRepository;
    private readonly IFundingProjectionService _projectionService;

    public ForecastEngine(
        IFundingRuleRepository fundingRuleRepository,
        IFundingProjectionService projectionService)
    {
        _fundingRuleRepository = fundingRuleRepository;
        _projectionService = projectionService;
    }

    public async Task<IReadOnlyList<ProjectionEntry>> GenerateForecastAsync(
        Guid accountId,
        DateOnly from,
        DateOnly to)
    {
        var rules =
            await _fundingRuleRepository
                .GetByAccountIdAsync(accountId);

        var entries = new List<ProjectionEntry>();

        foreach (var rule in rules)
        {
            entries.AddRange(
                _projectionService.Generate(
                    rule,
                    from,
                    to));
        }

        return entries
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Type)
            .ToList();
    }
}