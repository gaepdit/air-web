using FluentValidation;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.SourceTestReviews;

public class SourceTestReviewCommandValidator : AbstractValidator<SourceTestReviewCommandDto>
{
    public SourceTestReviewCommandValidator()
    {
        RuleFor(dto => dto.ReceivedByComplianceDate)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Date Received By Compliance cannot be in the future.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date is null || date >= dto.ReceivedByComplianceDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Date Received By Compliance.");
    }
}
