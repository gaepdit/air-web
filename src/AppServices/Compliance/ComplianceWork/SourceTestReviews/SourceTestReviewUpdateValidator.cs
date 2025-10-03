using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.SourceTestReviews;

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
