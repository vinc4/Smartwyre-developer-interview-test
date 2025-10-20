using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Abstractions;

public interface IRebateValidator
{
    bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request);
}