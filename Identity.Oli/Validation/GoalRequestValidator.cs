using FluentValidation;
using Identity.Oli.Models.Requests;

namespace Identity.Oli.Validation;

public class GoalRequestValidator : AbstractValidator<GoalRequest>
{
    public GoalRequestValidator()
    {
        RuleFor(goal => goal.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must be less than 100 characters.");

        RuleFor(goal => goal.Description)
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(goal => goal.Category)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(goal => goal.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
    }
}