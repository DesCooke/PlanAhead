using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Interfaces.Services;

public interface IDateCalculator
{
    DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate);

    IEnumerable<DateOnly> GenerateOccurrences(
        FundingRule rule,
        DateOnly from,
        DateOnly to);
}