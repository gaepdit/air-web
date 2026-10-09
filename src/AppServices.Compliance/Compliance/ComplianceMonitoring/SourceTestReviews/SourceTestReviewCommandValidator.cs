namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.SourceTestReviews;

public class SourceTestReviewCommandValidator : AbstractValidator<SourceTestReviewCommandDto>
{
    public SourceTestReviewCommandValidator()
    {
        RuleFor(dto => dto.ReceivedByComplianceDate)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The date received by compliance cannot be in the future.")
            .Must((dto, date) => dto.DateTestReviewComplete.HasValue &&
                                 date >= DateOnly.FromDateTime(dto.DateTestReviewComplete.Value))
            .WithMessage(
                "The date received by compliance cannot be earlier than the date the source test review was completed.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date >= dto.ReceivedByComplianceDate)
            .When(dto => dto.AcknowledgmentLetterDate.HasValue)
            .WithMessage("The acknowledgment letter date cannot be earlier than the date received by compliance.");
    }
}
