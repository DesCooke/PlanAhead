using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Dates;
using PlanAhead.Core.Services.Dates.Strategies;

public class DateCalculator : IDateCalculator
{
    private readonly Dictionary<Frequency, IFrequencyStrategy> _strategies;

    public DateCalculator(IEnumerable<IFrequencyStrategy> strategies)
    {
        _strategies =
            strategies.ToDictionary(s => s.Frequency);
    }

    public DateOnly? NextOccurrence(
        FundingRule rule,
        DateOnly currentDate)
    {
        return _strategies[rule.Frequency]
            .NextOccurrence(rule, currentDate);
    }

    public IEnumerable<DateOnly> GenerateOccurrences(
        FundingRule rule,
        DateOnly from,
        DateOnly to)
    {
        return _strategies[rule.Frequency]
            .GenerateOccurrences(rule, from, to);
    }
}