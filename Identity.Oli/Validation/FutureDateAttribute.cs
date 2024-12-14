using System.ComponentModel.DataAnnotations;

namespace Identity.Oli.Validation
{
    // Ensures that a DateTime value is in the future
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date <= DateTime.UtcNow)
                {
                    return new ValidationResult("The date must be in the future.");
                }
            }

            return ValidationResult.Success;
        }
    }
}