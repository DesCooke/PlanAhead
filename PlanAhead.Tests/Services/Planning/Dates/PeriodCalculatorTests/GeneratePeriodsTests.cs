using FluentAssertions;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Planning.Dates;

namespace PlanAhead.Tests.Services.Planning.Dates.PeriodCalculatorTests;

public class GeneratePeriodsTests
{
    private readonly PeriodCalculator _calculator = new();

    [Fact]
    public void Monthly_Should_Generate_All_Months()
    {
        var periods = _calculator.GeneratePeriods(
                Frequency.Monthly,
                new DateOnly(2027, 1, 15),
                new DateOnly(2027, 4, 15))
            .ToList();

        periods.Should().Equal(
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 2, 1),
            new DateOnly(2027, 3, 1),
            new DateOnly(2027, 4, 1));
    }

    [Fact]
    public void Quarterly_Should_Generate_All_Quarters()
    {
        var periods = _calculator.GeneratePeriods(
                Frequency.Quarterly,
                new DateOnly(2027, 2, 15),
                new DateOnly(2027, 10, 20))
            .ToList();

        periods.Should().Equal(
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 4, 1),
            new DateOnly(2027, 7, 1),
            new DateOnly(2027, 10, 1));
    }

    [Fact]
    public void BiAnnual_Should_Generate_Both_Halves()
    {
        var periods = _calculator.GeneratePeriods(
                Frequency.BiAnnual,
                new DateOnly(2027, 2, 1),
                new DateOnly(2028, 2, 1))
            .ToList();

        periods.Should().Equal(
            new DateOnly(2027, 1, 1),
            new DateOnly(2027, 7, 1),
            new DateOnly(2028, 1, 1));
    }

    [Fact]
    public void Annual_Should_Generate_All_Years()
    {
        var periods = _calculator.GeneratePeriods(
                Frequency.Annual,
                new DateOnly(2025, 6, 1),
                new DateOnly(2028, 3, 1))
            .ToList();

        periods.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2026, 1, 1),
            new DateOnly(2027, 1, 1),
            new DateOnly(2028, 1, 1));
    }

    [Fact]
    public void OneOff_Should_Return_Single_Date()
    {
        var periods = _calculator.GeneratePeriods(
                Frequency.OneOff,
                new DateOnly(2027, 6, 15),
                new DateOnly(2028, 6, 15))
            .ToList();

        periods.Should().ContainSingle();

        periods[0].Should().Be(new DateOnly(2027, 6, 15));
    }

    [Fact]
    public void Should_Return_Empty_When_From_Is_After_To()
    {
        var periods = _calculator.GeneratePeriods(
                Frequency.Monthly,
                new DateOnly(2028, 1, 1),
                new DateOnly(2027, 1, 1))
            .ToList();

        periods.Should().BeEmpty();
    }
}