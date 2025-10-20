using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Abstractions;

public interface IRebateCalculator
{
    decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}
