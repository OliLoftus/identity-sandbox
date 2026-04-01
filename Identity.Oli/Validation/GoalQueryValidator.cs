using FluentValidation;
using Identity.Oli.Models.Requests;

namespace Identity.Oli.Validation;

public class GoalQueryValidator : AbstractValidator<GoalQuery>
{
    private const int MaxPageSize = 100;
    private static readonly string[] AllowedSortFields = { "title", "category", "duedate", "iscompleted" };

    public GoalQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .When(query => query.Page.HasValue)
            .WithMessage("Page must be 1 or greater.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, MaxPageSize)
            .When(query => query.PageSize.HasValue)
            .WithMessage($"PageSize must be between 1 and {MaxPageSize}.");

        RuleFor(query => query)
            .Must(query => !query.DueFrom.HasValue || !query.DueTo.HasValue || query.DueFrom <= query.DueTo)
            .WithMessage("DueFrom must be less than or equal to DueTo.");

        RuleFor(query => query.SortBy)
            .Must(sortBy => string.IsNullOrWhiteSpace(sortBy) ||
                            AllowedSortFields.Contains(sortBy.Trim().ToLowerInvariant()))
            .WithMessage($"SortBy must be one of: {string.Join(", ", AllowedSortFields)}.");

        RuleFor(query => query.SortDir)
            .Must(sortDir => string.IsNullOrWhiteSpace(sortDir) ||
                             sortDir.Trim().Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                             sortDir.Trim().Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortDir must be 'asc' or 'desc'.");
    }
}
