using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validation;
using Smartwyre.DeveloperTest.Calculators;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateRepository _rebateRepository;
    private readonly IProductRepository _productRepository;
    private readonly IRebateValidator _validator;
    private readonly IRebateCalculatorFactory _calculatorFactory;
    public RebateService(IRebateRepository rebateRepository, IProductRepository productRepository, IRebateValidator validator, IRebateCalculatorFactory calculatorFactory)
    {
        _rebateRepository = rebateRepository;
        _productRepository = productRepository;
        _validator = validator;
        _calculatorFactory = calculatorFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        // 1. Data Access
        var rebate = _rebateRepository.GetRebate(request.RebateIdentifier);
        var product = _productRepository.GetProduct(request.ProductIdentifier);

        // 2. Validation
        if (!_validator.IsValid(rebate, product, request))
        {
            return new CalculateRebateResult { Success = false };
        }

        // 3. Calculation
        var calculator = _calculatorFactory.GetCalculator(rebate.Incentive);
        var rebateAmount = calculator.Calculate(rebate, product, request);

        // 4. Storage
        _rebateRepository.StoreCalculationResult(rebate, rebateAmount);

        return new CalculateRebateResult { Success = true };
    }

}
