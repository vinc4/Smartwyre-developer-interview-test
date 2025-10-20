using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Abstractions;

public interface IRebateCalculatorFactory
{
    IRebateCalculator GetCalculator(IncentiveType incentiveType);
}