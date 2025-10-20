using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedCashAmountCalculator : IRebateCalculator
{
    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        // For FixedCashAmount, we simply return the rebate amount
        // Validation should be done elsewhere
        return rebate.Amount;
    }
}