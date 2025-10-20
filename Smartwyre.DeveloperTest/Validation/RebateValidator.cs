using Smartwyre.DeveloperTest.Abstractions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validation;

public class RebateValidator : IRebateValidator
{
    public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        // Common validation - null checks
        if (rebate == null || product == null || request == null)
        {
            return false;
        }

        // Common validation - identifier checks
        if (string.IsNullOrWhiteSpace(rebate.Identifier) || 
            string.IsNullOrWhiteSpace(product.Identifier) ||
            string.IsNullOrWhiteSpace(request.RebateIdentifier) ||
            string.IsNullOrWhiteSpace(request.ProductIdentifier))
        {
            return false;
        }

        // Ensure request identifiers match the loaded entities
        if (request.RebateIdentifier != rebate.Identifier ||
            request.ProductIdentifier != product.Identifier)
        {
            return false;
        }

        // Validate based on incentive type
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
        // Check if product supports this incentive type
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
        {
            return false;
        }

        // Check if rebate amount is valid (must be positive - zero means no rebate which is invalid)
        if (rebate.Amount <= 0)
        {
            return false;
        }

        // For FixedCashAmount, volume and product price are not required for calculation
        // but volume should be positive if provided
        if (request.Volume < 0)
        {
            return false;
        }

        return true;
    }

    private bool ValidateFixedRateRebate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        // Check if product supports this incentive type
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
        {
            return false;
        }

        // Check if rebate percentage is valid (must be positive and reasonable)
        if (rebate.Percentage <= 0 || rebate.Percentage > 1.0m) // Assuming percentage is 0-1 (0-100%)
        {
            return false;
        }

        // Check if product price is valid (can be zero for free products, but not negative)
        if (product.Price < 0)
        {
            return false;
        }

        // Check if request volume is valid (must be positive for calculation)
        if (request.Volume <= 0)
        {
            return false;
        }

        return true;
    }

    private bool ValidateAmountPerUom(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        // Check if product supports this incentive type
        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
        {
            return false;
        }

        // Check if rebate amount is valid (must be positive)
        if (rebate.Amount <= 0)
        {
            return false;
        }

        // Check if request volume is valid (must be positive for calculation)
        if (request.Volume <= 0)
        {
            return false;
        }

        // For AmountPerUom, product price is not required for calculation
        // but should not be negative if present
        if (product.Price < 0)
        {
            return false;
        }

        return true;
    }
}