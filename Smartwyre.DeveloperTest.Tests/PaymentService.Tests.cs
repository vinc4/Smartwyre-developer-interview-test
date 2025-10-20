using System;
using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly Mock<IRebateRepository> _mockRebateRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IRebateValidator> _mockValidator;
    private readonly Mock<IRebateCalculatorFactory> _mockCalculatorFactory;
    private readonly Mock<IRebateCalculator> _mockCalculator;
    private readonly RebateService _rebateService;

    public RebateServiceTests()
    {
        _mockRebateRepository = new Mock<IRebateRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockValidator = new Mock<IRebateValidator>();
        _mockCalculatorFactory = new Mock<IRebateCalculatorFactory>();
        _mockCalculator = new Mock<IRebateCalculator>();

        _rebateService = new RebateService(
            _mockRebateRepository.Object,
            _mockProductRepository.Object,
            _mockValidator.Object,
            _mockCalculatorFactory.Object);
    }

    [Fact]
    public void Calculate_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 100
        };

        var rebate = new Rebate
        {
            Identifier = "REBATE001",
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 50
        };

        var product = new Product
        {
            Identifier = "PRODUCT001",
            Price = 10,
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        var calculatedAmount = 50m;

        _mockRebateRepository.Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);
        _mockProductRepository.Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);
        _mockValidator.Setup(x => x.IsValid(rebate, product, request))
            .Returns(true);
        _mockCalculatorFactory.Setup(x => x.GetCalculator(rebate.Incentive))
            .Returns(_mockCalculator.Object);
        _mockCalculator.Setup(x => x.Calculate(rebate, product, request))
            .Returns(calculatedAmount);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        _mockRebateRepository.Verify(x => x.StoreCalculationResult(rebate, calculatedAmount), Times.Once);
    }

    [Fact]
    public void Calculate_InvalidRequest_ReturnsFailureResult()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 100
        };

        var rebate = new Rebate { Identifier = "REBATE001" };
        var product = new Product { Identifier = "PRODUCT001" };

        _mockRebateRepository.Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);
        _mockProductRepository.Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);
        _mockValidator.Setup(x => x.IsValid(rebate, product, request))
            .Returns(false);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
        _mockRebateRepository.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_ValidRequest_CallsAllDependencies()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "REBATE001",
            ProductIdentifier = "PRODUCT001",
            Volume = 100
        };

        var rebate = new Rebate
        {
            Identifier = "REBATE001",
            Incentive = IncentiveType.FixedRateRebate
        };
        var product = new Product { Identifier = "PRODUCT001" };

        _mockRebateRepository.Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);
        _mockProductRepository.Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);
        _mockValidator.Setup(x => x.IsValid(rebate, product, request))
            .Returns(true);
        _mockCalculatorFactory.Setup(x => x.GetCalculator(rebate.Incentive))
            .Returns(_mockCalculator.Object);
        _mockCalculator.Setup(x => x.Calculate(rebate, product, request))
            .Returns(25m);

        // Act
        _rebateService.Calculate(request);

        // Assert
        _mockRebateRepository.Verify(x => x.GetRebate(request.RebateIdentifier), Times.Once);
        _mockProductRepository.Verify(x => x.GetProduct(request.ProductIdentifier), Times.Once);
        _mockValidator.Verify(x => x.IsValid(rebate, product, request), Times.Once);
        _mockCalculatorFactory.Verify(x => x.GetCalculator(rebate.Incentive), Times.Once);
        _mockCalculator.Verify(x => x.Calculate(rebate, product, request), Times.Once);
    }
}
