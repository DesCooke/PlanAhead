using PlanAhead.Core.Services.Dates.Strategies;
using PlanAhead.Tests.Builders;

namespace PlanAhead.Tests.Services.Dates.Strategies;

public class BiAnnualFrequencyStrategyTests
{
    private readonly BiAnnualFrequencyStrategy _strategy = new();

    [Fact]
    public void NextOccurrence_Should_Add_Six_Months()
    {
        var rule = FundingRuleBuilder
            .BiAnnual()
            .Build();

        var result = _strategy.NextOccurrence(
            rule,
            new DateOnly(2027, 2, 15));

        Assert.Equal(
            new DateOnly(2027, 8, 15),
            result);
    }

    [Fact]
    public void Should_Generate_Two_Dates_Per_Year()
    {
        var rule = FundingRuleBuilder
            .BiAnnual()
            .StartingOn(2027, 1, 1)
            .Build();

        var results = _strategy.GenerateOccurrences(
            rule,
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 12, 31))
            .ToList();

        Assert.Equal(2, results.Count);
    }
}