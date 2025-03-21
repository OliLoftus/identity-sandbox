using FluentValidation;
using Identity.Oli.Models;

namespace Identity.Oli.Validation;

public class ProgressUpdateValidator : AbstractValidator<ProgressUpdate>
{
    public ProgressUpdateValidator()
    {
        RuleFor(progress => progress.Update)
            .MaximumLength(500);

        RuleFor(progress => progress.PercentageComplete)
            .InclusiveBetween(0, 100);
    }
}