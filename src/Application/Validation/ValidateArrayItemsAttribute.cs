using System;
using System.ComponentModel.DataAnnotations;

namespace WZCNet.Validation;

public class ValidateArrayItemsAttribute : ValidationAttribute
{
    private readonly Type _itemType;
    private readonly bool _allowEmpty;
    
    public ValidateArrayItemsAttribute(Type itemType, bool allowEmpty = false)
    {
        _itemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
        _allowEmpty = allowEmpty;
    }
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success; // Let [Required] handle null validation
        
        
        var array = value as Array;
        if (array == null)
            return new ValidationResult("Value must be an array");
        
        // Check if array is empty and if empty arrays are not allowed
        if (array.Length == 0 && !_allowEmpty)
        {
            return new ValidationResult("Array cannot be empty when provided");
        }
        
        // Validate each item in the array
        for (int i = 0; i < array.Length; i++)
        {
            var item = array.GetValue(i);
            if (item != null)
            {
                var validationContextItem = new ValidationContext(item);
                var validationResults = new List<ValidationResult>();
                
                if (!Validator.TryValidateObject(item, validationContextItem, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    return new ValidationResult($"Item at index {i} validation failed: {errors}");
                }
            }
        }
        
        return ValidationResult.Success;
    }
}
