using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Interfaces.Strategies;

public interface IFrequencyStrategy
{
    Frequency Frequency { get; }

    DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate);

    IEnumerable<DateOnly> GenerateOccurrences(
        FundingRule rule,
        DateOnly from,
        DateOnly to);
}