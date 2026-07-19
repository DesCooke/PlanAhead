using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Services.Dates.Strategies;

public class BiAnnualFrequencyStrategy
    : BaseFrequencyStrategy
{
    public override Frequency Frequency =>
        Frequency.BiAnnual;

    public override DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate)
    {
        var next = currentDate.AddMonths(6);

        if (rule.EndDate.HasValue &&
            next > rule.EndDate.Value)
        {
            return null;
        }

        return next;
    }
}