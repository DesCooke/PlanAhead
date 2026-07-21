using FluentAssertions;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Planning.Dates;

namespace PlanAhead.Tests.Services.Planning.Dates.PeriodCalculatorTests;

public class GetNextPeriodTests
{
    private readonly PeriodCalculator _calculator = new();

    [Fact]
    public void Monthly_Should_Add_One_Month()
    {
        var result = _calculator.GetNextPeriod(
            Frequency.Monthly,
            new DateOnly(2027, 3, 1));

        result.Should().Be(new DateOnly(2027, 4, 1));
    }

    [Fact]
    public void Quarterly_Should_Add_Three_Months()
    {
        var result = _calculator.GetNextPeriod(
            Frequency.Quarterly,
            new DateOnly(2027, 4, 1));

        result.Should().Be(new DateOnly(2027, 7, 1));
    }

    [Fact]
    public void BiAnnual_Should_Add_Six_Months()
    {
        var result = _calculator.GetNextPeriod(
            Frequency.BiAnnual,
            new DateOnly(2027, 1, 1));

        result.Should().Be(new DateOnly(2027, 7, 1));
    }

    [Fact]
    public void Annual_Should_Add_One_Year()
    {
        var result = _calculator.GetNextPeriod(
            Frequency.Annual,
            new DateOnly(2027, 1, 1));

        result.Should().Be(new DateOnly(2028, 1, 1));
    }

    [Fact]
    public void OneOff_Should_Return_Same_Date()
    {
        var date = new DateOnly(2027, 6, 15);

        var result = _calculator.GetNextPeriod(
            Frequency.OneOff,
            date);

        result.Should().Be(date);
    }

    [Fact]
    public void Quarterly_Should_Cross_Year_Boundary()
    {
        var result = _calculator.GetNextPeriod(
            Frequency.Quarterly,
            new DateOnly(2027, 10, 1));

        result.Should().Be(new DateOnly(2028, 1, 1));
    }

    [Fact]
    public void Monthly_Should_Cross_Year_Boundary()
    {
        var result = _calculator.GetNextPeriod(
            Frequency.Monthly,
            new DateOnly(2027, 12, 1));

        result.Should().Be(new DateOnly(2028, 1, 1));
    }
}