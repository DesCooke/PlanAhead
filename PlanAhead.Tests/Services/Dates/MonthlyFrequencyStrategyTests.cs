using PlanAhead.Core.Services.Dates.Strategies;
using PlanAhead.Tests.Builders;

namespace PlanAhead.Tests.Services.Dates.Strategies;

public class MonthlyFrequencyStrategyTests
{
    private readonly MonthlyFrequencyStrategy _strategy = new();

    [Fact]
    public void NextOccurrence_Should_Add_One_Month()
    {
        var rule = FundingRuleBuilder
            .Monthly()
            .Build();

        var result = _strategy.NextOccurrence(
            rule,
            new DateOnly(2027, 1, 15));

        Assert.Equal(
            new DateOnly(2027, 2, 15),
            result);
    }

    [Fact]
    public void Should_Generate_Twelve_Monthly_Dates()
    {
        var rule = FundingRuleBuilder
            .Monthly()
            .StartingOn(2027, 1, 1)
            .Build();

        var results = _strategy.GenerateOccurrences(
            rule,
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 12, 31))
            .ToList();

        Assert.Equal(12, results.Count);
        Assert.Equal(new DateOnly(2027, 1, 1), results.First());
        Assert.Equal(new DateOnly(2027, 12, 1), results.Last());
    }

    [Fact]
    public void Should_Stop_When_End_Date_Reached()
    {
        var rule = FundingRuleBuilder
            .Monthly()
            .StartingOn(2027, 1, 1)
            .EndingOn(2027, 3, 1)
            .Build();

        var results = _strategy.GenerateOccurrences(
            rule,
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 12, 31))
            .ToList();

        Assert.Equal(3, results.Count);
    }
}