using Xunit;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class CalculatorTests
{
    [Fact]
    public void FixedCashAmountCalculator_Calculate_ReturnsRebateAmount()
    {
        // Arrange
        var calculator = new FixedCashAmountCalculator();
        var rebate = new Rebate { Amount = 50m };
        var product = new Product { Price = 10m };
        var request = new CalculateRebateRequest { Volume = 100m };

        // Act
        var result = calculator.Calculate(rebate, product, request);

        // Assert
        Assert.Equal(50m, result);
    }

    [Fact]
    public void FixedRateRebateCalculator_Calculate_ReturnsCorrectCalculation()
    {
        // Arrange
        var calculator = new FixedRateRebateCalculator();
        var rebate = new Rebate { Percentage = 0.1m }; // 10%
        var product = new Product { Price = 20m };
        var request = new CalculateRebateRequest { Volume = 5m };

        // Act
        var result = calculator.Calculate(rebate, product, request);

        // Assert
        // Expected: 20 * 0.1 * 5 = 10
        Assert.Equal(10m, result);
    }

    [Fact]
    public void AmountPerUomCalculator_Calculate_ReturnsCorrectCalculation()
    {
        // Arrange
        var calculator = new AmountPerUomCalculator();
        var rebate = new Rebate { Amount = 2.5m };
        var product = new Product { Price = 15m };
        var request = new CalculateRebateRequest { Volume = 4m };

        // Act
        var result = calculator.Calculate(rebate, product, request);

        // Assert
        // Expected: 2.5 * 4 = 10
        Assert.Equal(10m, result);
    }

    [Theory]
    [InlineData(IncentiveType.FixedCashAmount, typeof(FixedCashAmountCalculator))]
    [InlineData(IncentiveType.FixedRateRebate, typeof(FixedRateRebateCalculator))]
    [InlineData(IncentiveType.AmountPerUom, typeof(AmountPerUomCalculator))]
    public void RebateCalculatorFactory_GetCalculator_ReturnsCorrectCalculatorType(IncentiveType incentiveType, System.Type expectedType)
    {
        // Arrange
        var factory = new RebateCalculatorFactory();

        // Act
        var calculator = factory.GetCalculator(incentiveType);

        // Assert
        Assert.IsType(expectedType, calculator);
    }

    [Fact]
    public void RebateCalculatorFactory_GetCalculator_UnsupportedType_ThrowsNotSupportedException()
    {
        // Arrange
        var factory = new RebateCalculatorFactory();
        var unsupportedType = (IncentiveType)999;

        // Act & Assert
        Assert.Throws<System.NotSupportedException>(() => factory.GetCalculator(unsupportedType));
    }
}