using PlanAhead.Core.Services.Dates.Strategies;
using PlanAhead.Tests.Builders;

namespace PlanAhead.Tests.Services.Dates.Strategies;

public class QuarterlyFrequencyStrategyTests
{
    private readonly QuarterlyFrequencyStrategy _strategy = new();

    [Fact]
    public void NextOccurrence_Should_Add_Three_Months()
    {
        var rule = FundingRuleBuilder
            .Quarterly()
            .Build();

        var result = _strategy.NextOccurrence(
            rule,
            new DateOnly(2027, 2, 15));

        Assert.Equal(
            new DateOnly(2027, 5, 15),
            result);
    }

    [Fact]
    public void Should_Generate_Four_Quarterly_Dates()
    {
        var rule = FundingRuleBuilder
            .Quarterly()
            .StartingOn(2027, 1, 1)
            .Build();

        var results = _strategy.GenerateOccurrences(
            rule,
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 12, 31))
            .ToList();

        Assert.Equal(4, results.Count);

        Assert.Equal(new DateOnly(2027, 1, 1), results[0]);
        Assert.Equal(new DateOnly(2027, 4, 1), results[1]);
        Assert.Equal(new DateOnly(2027, 7, 1), results[2]);
        Assert.Equal(new DateOnly(2027, 10, 1), results[3]);
    }
}