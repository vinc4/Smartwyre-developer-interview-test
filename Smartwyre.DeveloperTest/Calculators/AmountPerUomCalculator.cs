using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class AmountPerUomCalculator : IRebateCalculator
{
    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        // Formula: rebate.Amount * request.Volume
        // Validation should be done elsewhere
        return rebate.Amount * request.Volume;
    }
}