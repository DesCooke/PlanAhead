using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Core.Services.Planning.Dates;

public class PeriodCalculator : IPeriodCalculator
{
    public DateOnly GetPeriodStart(
        Frequency frequency,
        DateOnly date)
    {
        return frequency switch
        {
            Frequency.Monthly =>
                new DateOnly(date.Year, date.Month, 1),

            Frequency.Quarterly =>
                new DateOnly(
                    date.Year,
                    GetQuarterStartMonth(date.Month),
                    1),

            Frequency.BiAnnual =>
                new DateOnly(
                    date.Year,
                    date.Month <= 6 ? 1 : 7,
                    1),

            Frequency.Annual =>
                new DateOnly(date.Year, 1, 1),

            Frequency.OneOff =>
                date,

            _ => throw new ArgumentOutOfRangeException(
                nameof(frequency),
                frequency,
                "Unsupported frequency.")
        };
    }

    public DateOnly GetNextPeriod(
        Frequency frequency,
        DateOnly currentPeriod)
    {
        return frequency switch
        {
            Frequency.Monthly =>
                currentPeriod.AddMonths(1),

            Frequency.Quarterly =>
                currentPeriod.AddMonths(3),

            Frequency.BiAnnual =>
                currentPeriod.AddMonths(6),

            Frequency.Annual =>
                currentPeriod.AddYears(1),

            Frequency.OneOff =>
                currentPeriod,

            _ => throw new ArgumentOutOfRangeException(
                nameof(frequency),
                frequency,
                "Unsupported frequency.")
        };
    }

    public bool IsPeriodStart(
        Frequency frequency,
        DateOnly date)
    {
        return GetPeriodStart(frequency, date) == date;
    }

    public DateOnly GetPreviousPeriod(
        Frequency frequency,
        DateOnly currentPeriod)
    {
        return frequency switch
        {
            Frequency.Monthly => currentPeriod.AddMonths(-1),
            Frequency.Quarterly => currentPeriod.AddMonths(-3),
            Frequency.BiAnnual => currentPeriod.AddMonths(-6),
            Frequency.Annual => currentPeriod.AddYears(-1),
            Frequency.OneOff => currentPeriod,
            _ => throw new ArgumentOutOfRangeException(nameof(frequency))
        };
    }

    public IEnumerable<DateOnly> GeneratePeriods(
        Frequency frequency,
        DateOnly from,
        DateOnly to)
    {
        if (from > to)
            yield break;

        var current = GetPeriodStart(
            frequency,
            from);

        while (current <= to)
        {
            yield return current;

            if (frequency == Frequency.OneOff)
                yield break;

            current = GetNextPeriod(
                frequency,
                current);
        }
    }

    private static int GetQuarterStartMonth(
        int month)
    {
        return ((month - 1) / 3) * 3 + 1;
    }
}