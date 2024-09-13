using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public class SourceTestReviewUpdateValidator : AbstractValidator<SourceTestReviewUpdateDto>
{
    public SourceTestReviewUpdateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCommandValidator());
        RuleFor(dto => dto).SetValidator(new SourceTestReviewCommandValidator());
    }
}
