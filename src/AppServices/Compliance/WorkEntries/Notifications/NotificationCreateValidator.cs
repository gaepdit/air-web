using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public class NotificationCreateValidator : AbstractValidator<NotificationCreateDto>
{
    public NotificationCreateValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());

        RuleFor(dto => dto.NotificationTypeId)
            .NotEmpty();

        RuleFor(dto => dto.ReceivedDate)
            .Must(date => date <= today)
            .WithMessage("The Received Date cannot be in the future.")
            .Must(date => date.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Received Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.")
            .Must((dto, date) => dto.SentDate is null || date >= dto.SentDate)
            .WithMessage("The Received Date must be later than the Sent Date.");

        RuleFor(dto => dto.SentDate)
            .Must(date => date is null || date <= today)
            .WithMessage("The Sent Date cannot be in the future.")
            .Must(date => date is null || date.Value.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Sent Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.");

        RuleFor(dto => dto.DueDate)
            .Must(date => date is null || date.Value.Year >= WorkEntry.EarliestWorkEntryYear)
            .WithMessage($"The Due Date cannot be earlier than {WorkEntry.EarliestWorkEntryYear}.");
    }
}
