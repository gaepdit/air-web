using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;

public class SourceTestReviewCreateValidator : AbstractValidator<SourceTestReviewCreateDto>
{
    private readonly IComplianceWorkService _service;

    public SourceTestReviewCreateValidator(
        IComplianceWorkService service,
        IValidator<IComplianceWorkCreateDto> complianceWorkCreateValidator,
        IValidator<SourceTestReviewCommandDto> sourceTestReviewCommandValidator)
    {
        _service = service;

        RuleFor(dto => dto).SetValidator(complianceWorkCreateValidator);
        RuleFor(dto => dto).SetValidator(sourceTestReviewCommandValidator);
        RuleFor(dto => dto.TestReportIsClosed).Equal(true);
        RuleFor(dto => dto.ReferenceNumber)
            .MustAsync(async (referenceNumber, token) =>
                await NotExist(referenceNumber, token).ConfigureAwait(false))
            .WithMessage("A compliance review already exists for this reference number.");
    }

    private async Task<bool> NotExist(int referenceNumber, CancellationToken token) =>
        !await _service.SourceTestReviewExistsAsync(referenceNumber, token).ConfigureAwait(false);
}
