using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validation;

public class RebateValidator : IRebateValidator
{
    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate == null || product == null || request == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(rebate.Identifier) || 
            string.IsNullOrWhiteSpace(product.Identifier) ||
            string.IsNullOrWhiteSpace(request.RebateIdentifier) ||
            string.IsNullOrWhiteSpace(request.ProductIdentifier))
        {
            return false;
        }

        if (request.RebateIdentifier != rebate.Identifier ||
            request.ProductIdentifier != product.Identifier)
        {
            return false;
        }

        return rebate.Incentive switch
        {
            IncentiveType.FixedCashAmount => ValidateFixedCashAmount(rebate, product, request),
            IncentiveType.FixedRateRebate => ValidateFixedRateRebate(rebate, product, request),
            IncentiveType.AmountPerUom => ValidateAmountPerUom(rebate, product, request),
            _ => false // Unknown incentive type
        };
    }

    private bool ValidateFixedCashAmount(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
        {
            return false;
        }

        if (rebate.Amount <= 0)
        {
            return false;
        }

        if (request.Volume < 0)
        {
            return false;
        }

        return true;
    }

    private bool ValidateFixedRateRebate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
        {
            return false;
        }

        if (rebate.Percentage <= 0 || rebate.Percentage > 1.0m) 
        {
            return false;
        }

        if (product.Price < 0)
        {
            return false;
        }

        if (request.Volume <= 0)
        {
            return false;
        }

        return true;
    }

    private bool ValidateAmountPerUom(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
        {
            return false;
        }

        if (rebate.Amount <= 0)
        {
            return false;
        }

        if (request.Volume <= 0)
        {
            return false;
        }

        if (product.Price < 0)
        {
            return false;
        }

        return true;
    }
}