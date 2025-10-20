using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Abstractions;

public interface IProductRepository
{
    Product GetProduct(string productIdentifier);
}