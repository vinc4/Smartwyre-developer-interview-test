using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateRepository : IRebateRepository
{
    private readonly RebateDataStore _dataStore;

    public RebateRepository()
    {
        _dataStore = new RebateDataStore();
    }

    public Rebate GetRebate(string rebateIdentifier)
    {
        return _dataStore.GetRebate(rebateIdentifier);
    }

    public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
    {
        _dataStore.StoreCalculationResult(rebate, rebateAmount);
    }
}