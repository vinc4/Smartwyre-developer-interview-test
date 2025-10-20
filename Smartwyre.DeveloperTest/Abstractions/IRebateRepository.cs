using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Abstractions;

public interface IRebateRepository
{
    Rebate GetRebate(string rebateIdentifier);
    void StoreCalculationResult(Rebate rebate, decimal rebateAmount);
}