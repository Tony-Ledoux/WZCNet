using System.ComponentModel.DataAnnotations;

namespace WZCNet.Validation;

public class DateRangeAttribute : ValidationAttribute
{
    public int MinYearsAgo { get; set; } = 0;
    public int MaxYearsAgo { get; set; } = int.MaxValue;
    public bool AllowFuture { get; set; } = false;
    public bool AllowPast { get; set; } = true;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly date)
        {
            return ValidationResult.Success; // Let [Required] handle nulls
        }
        if (date == default)
    {
        return new ValidationResult(
            ErrorMessage ?? $"{validationContext.DisplayName} is geen geldige datum.");
    }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (!AllowFuture && date > today)
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} mag niet in de toekomst zijn.");
        }

        if (!AllowPast && date < today)
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} mag niet in het verleden zijn.");
        }

        int yearsAgo = today.Year - date.Year;
        if (date > today.AddYears(-yearsAgo)) yearsAgo--;

        if (yearsAgo < MinYearsAgo)
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} moet minstens {MinYearsAgo} jaar geleden zijn.");
        }

        if (MaxYearsAgo != int.MaxValue && yearsAgo > MaxYearsAgo)
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} mag niet meer dan {MaxYearsAgo} jaar geleden zijn.");
        }

        return ValidationResult.Success;
    }
}