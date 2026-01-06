using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;

public class SourceTestReviewCreateValidator : AbstractValidator<SourceTestReviewCreateDto>
{
    private readonly IWorkEntryService _entryService;

    public SourceTestReviewCreateValidator(
        IWorkEntryService entryService,
        IValidator<IWorkEntryCreateDto> workEntryCreateValidator,
        IValidator<SourceTestReviewCommandDto> sourceTestReviewCommandValidator)
    {
        _entryService = entryService;

        RuleFor(dto => dto).SetValidator(workEntryCreateValidator);
        RuleFor(dto => dto).SetValidator(sourceTestReviewCommandValidator);
        RuleFor(dto => dto.TestReportIsClosed).Equal(true);
        RuleFor(dto => dto.ReferenceNumber)
            .MustAsync(async (referenceNumber, token) =>
                await NotExist(referenceNumber, token).ConfigureAwait(false))
            .WithMessage("A compliance review already exists for this reference number.");
    }

    private async Task<bool> NotExist(int referenceNumber, CancellationToken token) =>
        !await _entryService.SourceTestReviewExistsAsync(referenceNumber, token).ConfigureAwait(false);
}
