using PlanAhead.Core.Services.Dates.Strategies;
using PlanAhead.Tests.Builders;

namespace PlanAhead.Tests.Services.Dates.Strategies;

public class AnnualFrequencyStrategyTests
{
    private readonly AnnualFrequencyStrategy _strategy = new();

    [Fact]
    public void NextOccurrence_Should_Add_One_Year()
    {
        var rule = FundingRuleBuilder
            .Annual()
            .Build();

        var result = _strategy.NextOccurrence(
            rule,
            new DateOnly(2027, 12, 25));

        Assert.Equal(
            new DateOnly(2028, 12, 25),
            result);
    }

    [Fact]
    public void Should_Generate_One_Date()
    {
        var rule = FundingRuleBuilder
            .Annual()
            .StartingOn(2027, 12, 25)
            .Build();

        var results = _strategy.GenerateOccurrences(
            rule,
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 12, 31))
            .ToList();

        Assert.Single(results);
    }
}