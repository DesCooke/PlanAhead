using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Services.Dates;

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