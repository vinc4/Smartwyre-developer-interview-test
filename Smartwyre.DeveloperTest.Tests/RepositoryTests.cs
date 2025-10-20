using Xunit;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class RepositoryTests
{
    [Fact]
    public void RebateRepository_GetRebate_CallsDataStore()
    {
        // Arrange
        var repository = new RebateRepository();
        var rebateIdentifier = "REBATE001";

        // Act
        var result = repository.GetRebate(rebateIdentifier);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void RebateRepository_StoreCalculationResult_DoesNotThrow()
    {
        // Arrange
        var repository = new RebateRepository();
        var rebate = new Rebate { Identifier = "REBATE001" };
        var amount = 50m;

        // Act & Assert
        // Should not throw any exceptions
        repository.StoreCalculationResult(rebate, amount);
    }

    [Fact]
    public void ProductRepository_GetProduct_CallsDataStore()
    {
        // Arrange
        var repository = new ProductRepository();
        var productIdentifier = "PRODUCT001";

        // Act
        var result = repository.GetProduct(productIdentifier);

        // Assert
        Assert.NotNull(result);

    }
}