using FluentAssertions;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Planning;
using PlanAhead.Core.Services.Planning.Dates;
using PlanAhead.Tests.Builders;

namespace PlanAhead.Tests.Services.Planning;

public class FundingProjectionServiceTests
{
    private readonly FundingProjectionService _service;

    public FundingProjectionServiceTests()
    {
        _service = new FundingProjectionService(
            new PeriodCalculator());
    }

    [Fact]
    public void Should_Return_No_Entries_When_No_Rules_Exist()
    {
        var fund = FundBuilder.Create()
            .WithFrequency(Frequency.Annual)
            .Build();

        var projections = _service.Generate(
                fund,
                Enumerable.Empty<FundingRule>(),
                new DateOnly(2026, 1, 1),
                new DateOnly(2028, 12, 31))
            .ToList();

        projections.Should().BeEmpty();
    }

    [Fact]
    public void Should_Project_Single_Rule_For_All_Periods()
    {
        var fund = FundBuilder.Create()
            .WithFrequency(Frequency.Annual)
            .Build();

        var rule = FundingRuleBuilder.Create()
            .StartingOn(2026, 1, 1)
            .WithAmount(500)
            .Build();

        var projections = _service.Generate(
                fund,
                new[] { rule },
                new DateOnly(2026, 1, 1),
                new DateOnly(2028, 12, 31))
            .ToList();

        projections.Should().HaveCount(3);

        projections.Should()
            .OnlyContain(p => p.Amount == 500m);
    }

    [Fact]
    public void Should_Use_New_Rule_When_Period_Is_Reached()
    {
        var fund = FundBuilder.Create()
            .WithFrequency(Frequency.Annual)
            .Build();

        var rules = new[]
        {
            FundingRuleBuilder.Create()
                .StartingOn(2026,1,1)
                .WithAmount(500)
                .Build(),

            FundingRuleBuilder.Create()
                .StartingOn(2027,1,1)
                .WithAmount(600)
                .Build()
        };

        var projections = _service.Generate(
                fund,
                rules,
                new DateOnly(2026, 1, 1),
                new DateOnly(2028, 12, 31))
            .ToList();

        projections.Should().HaveCount(3);

        projections[0].Amount.Should().Be(500);

        projections[1].Amount.Should().Be(600);

        projections[2].Amount.Should().Be(600);
    }

    [Fact]
    public void Should_Continue_Using_Last_Rule_Until_Replaced()
    {
        var fund = FundBuilder.Create()
            .WithFrequency(Frequency.Annual)
            .Build();

        var rules = new[]
        {
            FundingRuleBuilder.Create()
                .StartingOn(2026,1,1)
                .WithAmount(500)
                .Build(),

            FundingRuleBuilder.Create()
                .StartingOn(2028,1,1)
                .WithAmount(700)
                .Build()
        };

        var projections = _service.Generate(
                fund,
                rules,
                new DateOnly(2026, 1, 1),
                new DateOnly(2029, 12, 31))
            .ToList();

        projections.Should().HaveCount(4);

        projections[0].Amount.Should().Be(500); //2026
        projections[1].Amount.Should().Be(500); //2027
        projections[2].Amount.Should().Be(700); //2028
        projections[3].Amount.Should().Be(700); //2029
    }

    [Fact]
    public void Should_Ignore_Future_Rules_Until_Their_Start_Period()
    {
        var fund = FundBuilder.Create()
            .WithFrequency(Frequency.Annual)
            .Build();

        var rules = new[]
        {
            FundingRuleBuilder.Create()
                .StartingOn(2030,1,1)
                .WithAmount(1000)
                .Build()
        };

        var projections = _service.Generate(
                fund,
                rules,
                new DateOnly(2026, 1, 1),
                new DateOnly(2029, 12, 31))
            .ToList();

        projections.Should().BeEmpty();
    }

    [Fact]
    public void Should_Order_Rules_By_StartDate()
    {
        var fund = FundBuilder.Create()
            .WithFrequency(Frequency.Annual)
            .Build();

        var rules = new[]
        {
            FundingRuleBuilder.Create()
                .StartingOn(2028,1,1)
                .WithAmount(700)
                .Build(),

            FundingRuleBuilder.Create()
                .StartingOn(2026,1,1)
                .WithAmount(500)
                .Build(),

            FundingRuleBuilder.Create()
                .StartingOn(2027,1,1)
                .WithAmount(600)
                .Build()
        };

        var projections = _service.Generate(
                fund,
                rules,
                new DateOnly(2026, 1, 1),
                new DateOnly(2028, 12, 31))
            .ToList();

        projections[0].Amount.Should().Be(500);
        projections[1].Amount.Should().Be(600);
        projections[2].Amount.Should().Be(700);
    }
}