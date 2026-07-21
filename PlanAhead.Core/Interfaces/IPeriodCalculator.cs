using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Interfaces.Services;

public interface IPeriodCalculator
{
    DateOnly GetPeriodStart(
        Frequency frequency,
        DateOnly date);

    DateOnly GetNextPeriod(
        Frequency frequency,
        DateOnly currentPeriod);

    IEnumerable<DateOnly> GeneratePeriods(
        Frequency frequency,
        DateOnly from,
        DateOnly to);
}