using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductRepository : IProductRepository
{
    private readonly ProductDataStore _dataStore;

    public ProductRepository()
    {
        _dataStore = new ProductDataStore();
    }

    public Product GetProduct(string productIdentifier)
    {
        return _dataStore.GetProduct(productIdentifier);
    }
}