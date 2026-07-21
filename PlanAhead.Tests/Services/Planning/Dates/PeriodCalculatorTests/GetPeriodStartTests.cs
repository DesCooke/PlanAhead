using FluentAssertions;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Planning.Dates;

namespace PlanAhead.Tests.Services.Planning.Dates.PeriodCalculatorTests;

public class GetPeriodStartTests
{
    private readonly PeriodCalculator _calculator = new();

    [Fact]
    public void Monthly_Should_Return_First_Day_Of_Month()
    {
        var result = _calculator.GetPeriodStart(
            Frequency.Monthly,
            new DateOnly(2027, 3, 15));

        result.Should().Be(new DateOnly(2027, 3, 1));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 4)]
    [InlineData(5, 4)]
    [InlineData(6, 4)]
    [InlineData(7, 7)]
    [InlineData(8, 7)]
    [InlineData(9, 7)]
    [InlineData(10, 10)]
    [InlineData(11, 10)]
    [InlineData(12, 10)]
    public void Quarterly_Should_Return_First_Day_Of_Quarter(
        int month,
        int expectedMonth)
    {
        var result = _calculator.GetPeriodStart(
            Frequency.Quarterly,
            new DateOnly(2027, month, 15));

        result.Should().Be(
            new DateOnly(2027, expectedMonth, 1));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(5, 1)]
    [InlineData(6, 1)]
    [InlineData(7, 7)]
    [InlineData(8, 7)]
    [InlineData(9, 7)]
    [InlineData(10, 7)]
    [InlineData(11, 7)]
    [InlineData(12, 7)]
    public void BiAnnual_Should_Return_First_Day_Of_Half_Year(
        int month,
        int expectedMonth)
    {
        var result = _calculator.GetPeriodStart(
            Frequency.BiAnnual,
            new DateOnly(2027, month, 15));

        result.Should().Be(
            new DateOnly(2027, expectedMonth, 1));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(7)]
    [InlineData(12)]
    public void Annual_Should_Return_First_Day_Of_Year(
        int month)
    {
        var result = _calculator.GetPeriodStart(
            Frequency.Annual,
            new DateOnly(2027, month, 15));

        result.Should().Be(
            new DateOnly(2027, 1, 1));
    }

    [Fact]
    public void OneOff_Should_Return_Same_Date()
    {
        var date = new DateOnly(2027, 6, 15);

        var result = _calculator.GetPeriodStart(
            Frequency.OneOff,
            date);

        result.Should().Be(date);
    }
}