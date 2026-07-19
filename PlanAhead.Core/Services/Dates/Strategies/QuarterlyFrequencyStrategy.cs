using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Services.Dates.Strategies;

public class QuarterlyFrequencyStrategy
    : BaseFrequencyStrategy
{
    public override Frequency Frequency =>
        Frequency.Quarterly;

    public override DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate)
    {
        var next = currentDate.AddMonths(3);

        if (rule.EndDate.HasValue &&
            next > rule.EndDate.Value)
        {
            return null;
        }

        return next;
    }
}