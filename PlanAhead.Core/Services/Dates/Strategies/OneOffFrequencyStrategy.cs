using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Services.Dates.Strategies;

public class OneOffFrequencyStrategy
    : BaseFrequencyStrategy
{
    public override Frequency Frequency =>
        Frequency.OneOff;

    public override DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate)
    {
        return null;
    }

    public override IEnumerable<DateOnly> GenerateOccurrences(
        FundingRule rule,
        DateOnly from,
        DateOnly to)
    {
        if (rule.StartDate >= from &&
            rule.StartDate <= to)
        {
            yield return rule.StartDate;
        }
    }
}