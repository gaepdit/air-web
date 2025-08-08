using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public class SourceTestReviewUpdateValidator : AbstractValidator<SourceTestReviewUpdateDto>
{
    public SourceTestReviewUpdateValidator(
        IValidator<IWorkEntryCommandDto> workEntryCommandValidator,
        IValidator<SourceTestReviewCommandDto> sourceTestReviewCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCommandValidator);
        RuleFor(dto => dto).SetValidator(sourceTestReviewCommandValidator);
    }
}
