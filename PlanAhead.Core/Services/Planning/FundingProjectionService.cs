using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Services.Planning;

public class FundingProjectionService : IFundingProjectionService
{
    private readonly IPeriodCalculator _periodCalculator;

    public FundingProjectionService(
        IPeriodCalculator periodCalculator)
    {
        _periodCalculator = periodCalculator;
    }

    public IEnumerable<ProjectionEntry> Generate(
        Fund fund,
        IEnumerable<FundingRule> rules,
        DateOnly from,
        DateOnly to)
    {
        var orderedRules = rules
            .OrderBy(r => r.StartDate)
            .ToList();

        foreach (var period in _periodCalculator.GeneratePeriods(
                     fund.Frequency,
                     from,
                     to))
        {
            var rule = GetRuleForPeriod(
                orderedRules,
                period);

            if (rule == null)
                continue;

            yield return new ProjectionEntry
            {
                AccountId = fund.AccountId,
                FundId = fund.Id,
                Date = period,
                Amount = rule.Amount,
                Type = ProjectionType.Funding
            };
        }
    }

    private static FundingRule? GetRuleForPeriod(
        IReadOnlyList<FundingRule> rules,
        DateOnly period)
    {
        return rules
            .Where(r => r.StartDate <= period)
            .OrderByDescending(r => r.StartDate)
            .FirstOrDefault();
    }
}