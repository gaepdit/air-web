using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public class NotificationCreateValidator : AbstractValidator<NotificationCreateDto>
{
    public NotificationCreateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());
        RuleFor(dto => dto).SetValidator(new NotificationCommandValidator());
    }
}
