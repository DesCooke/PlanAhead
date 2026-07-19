using PlanAhead.Core.Services.Dates.Strategies;
using PlanAhead.Tests.Builders;

namespace PlanAhead.Tests.Services.Dates.Strategies;

public class OneOffFrequencyStrategyTests
{
    private readonly OneOffFrequencyStrategy _strategy = new();

    [Fact]
    public void NextOccurrence_Should_Return_Null()
    {
        var rule = FundingRuleBuilder
            .OneOff()
            .Build();

        var result = _strategy.NextOccurrence(
            rule,
            rule.StartDate);

        Assert.Null(result);
    }

    [Fact]
    public void Should_Generate_Single_Date()
    {
        var rule = FundingRuleBuilder
            .OneOff()
            .StartingOn(2027, 8, 15)
            .Build();

        var results = _strategy.GenerateOccurrences(
            rule,
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 12, 31))
            .ToList();

        Assert.Single(results);
        Assert.Equal(new DateOnly(2027, 8, 15), results.Single());
    }

    [Fact]
    public void Should_Return_No_Dates_When_Outside_Range()
    {
        var rule = FundingRuleBuilder
            .OneOff()
            .StartingOn(2028, 8, 15)
            .Build();

        var results = _strategy.GenerateOccurrences(
            rule,
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 12, 31))
            .ToList();

        Assert.Empty(results);
    }
}