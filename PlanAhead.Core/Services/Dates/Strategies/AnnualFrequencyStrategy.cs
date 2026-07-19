using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Services.Dates.Strategies;

public class AnnualFrequencyStrategy
    : BaseFrequencyStrategy
{
    public override Frequency Frequency =>
        Frequency.Annual;

    public override DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate)
    {
        var next = currentDate.AddYears(1);

        if (rule.EndDate.HasValue &&
            next > rule.EndDate.Value)
        {
            return null;
        }

        return next;
    }
}