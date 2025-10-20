using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Calculators;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    public IRebateCalculator GetCalculator(IncentiveType incentiveType)
    {
        return incentiveType switch
        {
            IncentiveType.FixedCashAmount => new FixedCashAmountCalculator(),
            IncentiveType.FixedRateRebate => new FixedRateRebateCalculator(),
            IncentiveType.AmountPerUom => new AmountPerUomCalculator(),
            _ => throw new NotSupportedException($"Incentive type {incentiveType} is not supported.")
        };
    }
}