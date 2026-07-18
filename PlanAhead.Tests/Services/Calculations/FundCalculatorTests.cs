using PlanAhead.Core.Services.Calculations;

namespace PlanAhead.Tests.Services.Calculations;

public class FundCalculatorTests
{
    private readonly IFundCalculator _calculator = new FundCalculator();

    private const decimal ChristmasTarget = 500m;
    private const decimal ChristmasFunded = 450m;
    private const decimal ChristmasSpent = 320m;

    [Fact]
    public void CalculateFundBalance_Should_Return_Funded_Minus_Spent()
    {
        // Arrange
        const decimal funded = 450m;
        const decimal spent = 320m;

        // Act
        var result = _calculator.CalculateFundBalance(funded, spent);

        // Assert
        Assert.Equal(130m, result);
    }

    [Fact]
    public void CalculateRemainingSpend_Should_Return_Target_Minus_Spent()
    {
        // Arrange
        const decimal target = 500m;
        const decimal spent = 320m;

        // Act
        var result = _calculator.CalculateRemainingSpend(target, spent);

        // Assert
        Assert.Equal(180m, result);
    }

    [Fact]
    public void CalculateRemainingSpend_Should_Not_Return_Negative_Value()
    {
        // Arrange
        const decimal target = 500m;
        const decimal spent = 600m;

        // Act
        var result = _calculator.CalculateRemainingSpend(target, spent);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateFundingShortfall_Should_Return_RemainingSpend_Minus_FundBalance()
    {
        // Arrange
        const decimal fundBalance = 130m;
        const decimal remainingSpend = 180m;

        // Act
        var result = _calculator.CalculateFundingShortfall(
            fundBalance,
            remainingSpend);

        // Assert
        Assert.Equal(50m, result);
    }

    [Fact]
    public void CalculateFundingShortfall_Should_Not_Return_Negative_Value()
    {
        // Arrange
        const decimal fundBalance = 400m;
        const decimal remainingSpend = 180m;

        // Act
        var result = _calculator.CalculateFundingShortfall(
            fundBalance,
            remainingSpend);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateFundingPercentage_Should_Return_Ninety_Percent()
    {
        // Arrange
        const decimal funded = 450m;
        const decimal target = 500m;

        // Act
        var result = _calculator.CalculateFundingPercentage(
            funded,
            target);

        // Assert
        Assert.Equal(90m, result);
    }

    [Fact]
    public void CalculateFundingPercentage_Should_Return_OneHundred_When_Target_Is_Zero()
    {
        // Arrange
        const decimal funded = 100m;
        const decimal target = 0m;

        // Act
        var result = _calculator.CalculateFundingPercentage(
            funded,
            target);

        // Assert
        Assert.Equal(100m, result);
    }

    [Fact]
    public void Christmas_Fund_Should_Have_A_Shortfall_Of_Fifty_Pounds()
    {
        // Arrange
        var fundBalance = _calculator.CalculateFundBalance(
            ChristmasFunded,
            ChristmasSpent);

        var remainingSpend = _calculator.CalculateRemainingSpend(
            ChristmasTarget,
            ChristmasSpent);

        // Act
        var shortfall = _calculator.CalculateFundingShortfall(
            fundBalance,
            remainingSpend);

        // Assert
        Assert.Equal(50m, shortfall);
    }
}