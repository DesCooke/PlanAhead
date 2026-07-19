using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Services.Dates.Strategies;

public abstract class BaseFrequencyStrategy : IFrequencyStrategy
{
    public abstract Frequency Frequency { get; }

    public abstract DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate);

    public virtual IEnumerable<DateOnly> GenerateOccurrences(
        FundingRule rule,
        DateOnly from,
        DateOnly to)
    {
        if (to < from)
            yield break;

        if (rule.StartDate > to)
            yield break;

        var current =
            rule.StartDate > from
                ? rule.StartDate
                : from;

        while (current <= to)
        {
            if (!rule.EndDate.HasValue ||
                current <= rule.EndDate.Value)
            {
                yield return current;
            }

            var next = NextOccurrence(rule, current);

            if (!next.HasValue)
                yield break;

            current = next.Value;
        }
    }
}