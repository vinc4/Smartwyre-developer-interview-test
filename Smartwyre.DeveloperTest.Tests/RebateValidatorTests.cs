using Xunit;
using Smartwyre.DeveloperTest.Validation;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateValidatorTests
{
    private readonly RebateValidator _validator;

    public RebateValidatorTests()
    {
        _validator = new RebateValidator();
    }

    [Fact]
    public void IsValid_NullRebate_ReturnsFalse()
    {
        // Arrange
        var product = new Product { Identifier = "PRODUCT001" };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001", 
            ProductIdentifier = "PRODUCT001" 
        };

        // Act
        var result = _validator.IsValid(null, product, request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_NullProduct_ReturnsFalse()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "REBATE001" };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001", 
            ProductIdentifier = "PRODUCT001" 
        };

        // Act
        var result = _validator.IsValid(rebate, null, request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_NullRequest_ReturnsFalse()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "REBATE001" };
        var product = new Product { Identifier = "PRODUCT001" };

        // Act
        var result = _validator.IsValid(rebate, product, null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_MismatchedIdentifiers_ReturnsFalse()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "REBATE001" };
        var product = new Product { Identifier = "PRODUCT001" };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE002", // Different identifier
            ProductIdentifier = "PRODUCT001",
            Volume = 10
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_FixedCashAmount_ValidData_ReturnsTrue()
    {
        // Arrange
        var rebate = new Rebate 
        { 
            Identifier = "REBATE001",
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 50m
        };
        var product = new Product 
        { 
            Identifier = "PRODUCT001",
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 10
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_FixedCashAmount_UnsupportedIncentive_ReturnsFalse()
    {
        // Arrange
        var rebate = new Rebate 
        { 
            Identifier = "REBATE001",
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 50m
        };
        var product = new Product 
        { 
            Identifier = "PRODUCT001",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate // Different incentive
        };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 10
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_FixedCashAmount_ZeroAmount_ReturnsFalse()
    {
        // Arrange
        var rebate = new Rebate 
        { 
            Identifier = "REBATE001",
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 0m // Invalid amount
        };
        var product = new Product 
        { 
            Identifier = "PRODUCT001",
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 10
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_FixedRateRebate_ValidData_ReturnsTrue()
    {
        // Arrange
        var rebate = new Rebate 
        { 
            Identifier = "REBATE001",
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.1m
        };
        var product = new Product 
        { 
            Identifier = "PRODUCT001",
            Price = 10m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 5
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.1)]
    [InlineData(1.1)] // Over 100%
    public void IsValid_FixedRateRebate_InvalidPercentage_ReturnsFalse(decimal percentage)
    {
        // Arrange
        var rebate = new Rebate 
        { 
            Identifier = "REBATE001",
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = percentage
        };
        var product = new Product 
        { 
            Identifier = "PRODUCT001",
            Price = 10m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 5
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_AmountPerUom_ValidData_ReturnsTrue()
    {
        // Arrange
        var rebate = new Rebate 
        { 
            Identifier = "REBATE001",
            Incentive = IncentiveType.AmountPerUom,
            Amount = 2.5m
        };
        var product = new Product 
        { 
            Identifier = "PRODUCT001",
            Price = 10m,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 4
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_AmountPerUom_ZeroVolume_ReturnsFalse()
    {
        // Arrange
        var rebate = new Rebate 
        { 
            Identifier = "REBATE001",
            Incentive = IncentiveType.AmountPerUom,
            Amount = 2.5m
        };
        var product = new Product 
        { 
            Identifier = "PRODUCT001",
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };
        var request = new CalculateRebateRequest 
        { 
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 0 // Invalid volume
        };

        // Act
        var result = _validator.IsValid(rebate, product, request);

        // Assert
        Assert.False(result);
    }
}