using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public class SourceTestReviewCommandValidator : AbstractValidator<SourceTestReviewCommandDto>
{
    public SourceTestReviewCommandValidator()
    {
        RuleFor(dto => dto.ReceivedByCompliance)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Date Received By Compliance cannot be in the future.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date is null || date >= dto.ReceivedByCompliance)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Date Received By Compliance.");
    }
}
