using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedRateRebateCalculator : IRebateCalculator
{
    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        // Formula: product.Price * rebate.Percentage * request.Volume
        // Validation should be done elsewhere
        return product.Price * rebate.Percentage * request.Volume;
    }
}