using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

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
        RuleFor(dto => dto).MustAsync(async (dto, token) =>
            await NotExist(dto.ReferenceNumber, token).ConfigureAwait(false));
    }

    private async Task<bool> NotExist(int referenceNumber, CancellationToken token) =>
        !await _entryService.SourceTestReviewExistsAsync(referenceNumber, token).ConfigureAwait(false);
}
