namespace PlanAhead.Core.Services.Calculations;

public class FundCalculator : IFundCalculator
{
    public decimal CalculateFundBalance(
        decimal funded,
        decimal spent)
    {
        return funded - spent;
    }

    public decimal CalculateRemainingSpend(
        decimal target,
        decimal spent)
    {
        return Math.Max(0, target - spent);
    }

    public decimal CalculateFundingShortfall(
        decimal fundBalance,
        decimal remainingSpend)
    {
        return Math.Max(0, remainingSpend - fundBalance);
    }

    public decimal CalculateFundingPercentage(
        decimal funded,
        decimal target)
    {
        if (target <= 0)
            return 100m;

        return Math.Round(
            (funded / target) * 100m,
            1,
            MidpointRounding.AwayFromZero);
    }
}