using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Dates.Strategies;

public class MonthlyFrequencyStrategy : BaseFrequencyStrategy
{
    public override Frequency Frequency => Frequency.Monthly;

    public override DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate)
    {
        var next = currentDate.AddMonths(1);

        if (rule.EndDate.HasValue &&
            next > rule.EndDate.Value)
        {
            return null;
        }

        return next;
    }

}