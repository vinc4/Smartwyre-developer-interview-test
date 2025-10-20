using System;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Validation;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Smartwyre Rebate Calculator ===");
        Console.WriteLine();


        var rebateRepository = new RebateRepository();
        var productRepository = new ProductRepository();
        var validator = new RebateValidator();
        var calculatorFactory = new RebateCalculatorFactory();

 
        var rebateService = new RebateService(rebateRepository, productRepository, validator, calculatorFactory);


        Console.WriteLine("Testing Rebate Service...");
        
        try
        {
            var request = new CalculateRebateRequest
            {
                RebateIdentifier = "REBATE001",
                ProductIdentifier = "PRODUCT001", 
                Volume = 100
            };

            var result = rebateService.Calculate(request);
            
            Console.WriteLine($"Rebate calculation result: {(result.Success ? "SUCCESS" : "FAILED")}");
            
            if (!result.Success)
            {
                Console.WriteLine("Note: This failure is expected because the DataStore returns empty objects.");
                Console.WriteLine("In a real implementation, the DataStore would return actual data.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
