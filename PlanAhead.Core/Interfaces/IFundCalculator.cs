namespace PlanAhead.Core.Services.Calculations;

public interface IFundCalculator
{
    decimal CalculateFundBalance(decimal funded, decimal spent);

    decimal CalculateRemainingSpend(decimal target, decimal spent);

    decimal CalculateFundingShortfall(
        decimal fundBalance,
        decimal remainingSpend);

    decimal CalculateFundingPercentage(
        decimal funded,
        decimal target);
}