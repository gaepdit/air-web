using AirWeb.Domain;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public class NotificationCommandValidator : AbstractValidator<NotificationCommandDto>
{
    public NotificationCommandValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.NotificationTypeId)
            .NotEmpty();

        RuleFor(dto => dto.ReceivedDate)
            .Must(date => date <= today)
            .WithMessage("The Received Date cannot be in the future.")
            .Must(date => date.Year >= ComplianceConstants.EarliestWorkEntryYear)
            .WithMessage($"The Received Date cannot be earlier than {ComplianceConstants.EarliestWorkEntryYear}.")
            .Must((dto, date) => dto.SentDate is null || date >= dto.SentDate)
            .WithMessage("The Received Date must be later than the Sent Date.");

        RuleFor(dto => dto.SentDate)
            .Must(date => date is null || date <= today)
            .WithMessage("The Sent Date cannot be in the future.")
            .Must(date => date is null || date.Value.Year >= ComplianceConstants.EarliestWorkEntryYear)
            .WithMessage($"The Sent Date cannot be earlier than {ComplianceConstants.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.DueDate)
            .Must(date => date is null || date <= today.AddYears(1))
            .WithMessage("The Due Date cannot be more than a year in the future.")
            .Must(date => date is null || date.Value.Year >= ComplianceConstants.EarliestWorkEntryYear)
            .WithMessage($"The Due Date cannot be earlier than {ComplianceConstants.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.AcknowledgmentLetterDate)
            .Must((dto, date) => date is null || date >= dto.ReceivedDate)
            .WithMessage("The Acknowledgment Letter Date cannot be earlier than the Received Date.");
    }
}
